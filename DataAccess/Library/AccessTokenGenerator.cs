using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Concrete.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DataAccess.Library;

public class AccessTokenGenerator
{
    private NorthwindContext Context { get; set; }
    private IConfiguration Configuration { get; set; }
    private User User { get; set; }

    /// <summary>
    /// Class Constructor.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    /// <param name="user"></param>
    /// <returns></returns>
    public AccessTokenGenerator(NorthwindContext context, IConfiguration configuration, User user)
    {
        Configuration = configuration;
        Context = context;
        User = user;
    }

    /// <summary>
    /// Kullanıcıya tanımlı tokenı döner;
    /// Token yoksa oluşturur.
    /// Expire olmuşsa update eder.
    /// </summary>
    /// <returns></returns>
    public UserToken GetToken()
    {
        UserToken userTokens;
        TokenInfo tokenInfo;

        // Kullanıcıya ait token var mı kontrol edilir.
        if (Context.ApplicationUserToken.Any(x => x.UserId == User.Id))
        {
            // İlgili token bilgileri bulunur.
            userTokens = Context.ApplicationUserToken.SingleOrDefault(x => x.UserId == User.Id)!;

            // Expire olmuş ise yeni token oluşturup günceller.
            if (userTokens.ExpireDate <= DateTime.Now)
            {
                // Create new token
                tokenInfo = GenerateToken();

                userTokens.ExpireDate = tokenInfo.ExpireDate;
                userTokens.Value = tokenInfo.Token;

                Context.ApplicationUserToken.Update(userTokens);
            }
        }
        else
        {
            // Create new token
            tokenInfo = GenerateToken();

            userTokens = new UserToken
            {
                UserId = User.Id,
                LoginProvider = "SystemAPI",
                Name = User.FirstName + User.LastName,

                ExpireDate = tokenInfo.ExpireDate,
                Value = tokenInfo.Token
            };

            Context.ApplicationUserToken.Add(userTokens);
        }

        Context.SaveChangesAsync();

        return userTokens;
    }

    /// <summary>
    /// Kullanıcıya ait tokenı siler.
    /// </summary>
    /// <returns></returns>
    public async Task<bool> DeleteToken()
    {
        var returnResult = true;

        try
        {
            // Kullanıcıya ait önceden oluşturulmuş bir token var mı kontrol edilir.
            if (Context.ApplicationUserToken.Any(x => x.UserId == User.Id))
            {
                var userTokens = Context.ApplicationUserToken.FirstOrDefault(x => x.UserId == User.Id);

                if (userTokens != null) Context.ApplicationUserToken.Remove(userTokens);
            }

            await Context.SaveChangesAsync();
        }
        catch (Exception)
        {
            returnResult = false;
        }

        return returnResult;
    }

    /// <summary>
    /// Yeni token oluşturur.
    /// </summary>
    /// <returns></returns>
    private TokenInfo GenerateToken()
    {
        var expireDate = DateTime.Now.AddMinutes(15);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Configuration["Application:Secret"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = Configuration["Application:Audience"],
            Issuer = Configuration["Application:Issuer"],
            Subject = new ClaimsIdentity(new[]
            {
                // Claim tanımları yapılır.
                // Burada en önemlisi Id ve emaildir.
                // Id üzerinden, aktif kullanıcıyı buluyor olacağız.

                new Claim(ClaimTypes.NameIdentifier, User.Id),
                new Claim(ClaimTypes.Name, User.FirstName + User.LastName),
                new Claim(ClaimTypes.Email, User.Email)
            }),

            // ExpireDate
            Expires = expireDate,

            // Şifreleme türünü belirtiyoruz: HmacSha256Signature
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        var tokenInfo = new TokenInfo
        {
            Token = tokenString,
            ExpireDate = expireDate
        };

        return tokenInfo;
    }
}