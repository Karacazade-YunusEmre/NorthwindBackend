using System.ComponentModel;
using Core.Entities.Abstract;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Concrete.Authentication;

public class UserToken : IdentityUserToken<string>, IAuthenticationEntity
{
    [DisplayName("GeçerlilikTarihi")] public DateTime ExpireDate { get; set; }
}