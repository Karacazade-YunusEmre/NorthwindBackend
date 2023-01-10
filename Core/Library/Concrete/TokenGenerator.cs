using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Entities.Concrete.Authentication;
using Core.Library.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Library.Concrete;

public class TokenGenerator : ITokenGenerator
{
    private readonly IConfiguration _configuration;

    public TokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Kullanıcıya tanımlı tokenı döner;
    /// Token yoksa oluşturur.
    /// Expire olmuşsa update eder.
    /// </summary>
    /// <returns></returns>
    public async Task<UserToken> GetToken<TContext>(User user) where TContext : DbContext, new()
    {
        await using var context = new TContext();
        UserToken currentToken;

        var existsToken = context.Set<UserToken>().Any(u => u.UserId == user.Id);

        // Kullanıcıya ait token var mı kontrol edilir.
        if (existsToken)
        {
            // ilgili token bilgileri bulunur
            currentToken = context.Set<UserToken>().SingleOrDefault(u => u.UserId == user.Id)!;

            // Token expire olmuş ise yeni token oluşturup güncellenir
            if (currentToken.ExpireDate <= DateTime.Now)
            {
                var newToken = GenerateToken(user);

                currentToken.Value = newToken.Value;
                currentToken.ExpireDate = newToken.ExpireDate;

                context.Set<UserToken>().Update(currentToken);
            }
        }
        else
        {
            // Yeni token oluşturulur
            var newToken = GenerateToken(user);

            currentToken = new UserToken()
            {
                UserId = newToken.UserId,
                ExpireDate = newToken.ExpireDate,
                Value = newToken.Value,
                LoginProvider = "SystemAPI",
                Name = user.FirstName + " " + user.LastName,
            };

            // Yeni token tabloya kaydedilir.
            context.Set<UserToken>().Add(currentToken);
        }

        await context.SaveChangesAsync();
        return currentToken;
    }

    /// <summary>
    /// Kullanıcıya ait tokenı siler.
    /// </summary>
    /// <returns></returns>
    public async Task<bool> DeleteToken<TContext>(User user) where TContext : DbContext, new()
    {
        await using var context = new TContext();
        var returnResult = true;

        try
        {
            var existsToken = context.Set<UserToken>().Any(u => u.UserId == user.Id);

            // Kullanıcıya ait token var mı kontrol edilir
            if (existsToken)
            {
                // Kullanıcıya ait tanımlı token bulunur
                var currentToken = context.Set<UserToken>().SingleOrDefault(u => u.UserId == user.Id);

                // Mevcut token silinir
                if (currentToken != null) context.Set<UserToken>().Remove(currentToken);

                await context.SaveChangesAsync();
            }
        }
        catch
        {
            returnResult = false;
        }

        return returnResult;
    }

    /// <summary>
    /// Yeni token oluşturur.
    /// </summary>
    private UserToken GenerateToken(User user)
    {
        var expireDate = DateTime.Now.AddMinutes(15);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Application:Secret"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Audience = _configuration["Application:Audience"],
            Issuer = _configuration["Application:Issuer"],
            Subject = new ClaimsIdentity(new[]
            {
                // Claim tanımları yapılır.
                // Burada en önemlisi Id ve emaildir.
                // Id üzerinden, aktif kullanıcıyı buluyor olacağız.

                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Email, user.Email)
            }),

            // ExpireDate
            Expires = expireDate,

            // Şifreleme türünü belirtiyoruz: HmacSha256Signature
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        var newToken = new UserToken()
        {
            UserId = user.Id,
            Value = tokenString,
            ExpireDate = expireDate,
        };

        return newToken;
    }
}