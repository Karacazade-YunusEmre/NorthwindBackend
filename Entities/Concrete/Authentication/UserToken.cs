using System.ComponentModel;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace Entities.Concrete.Authentication;

public class UserToken : IdentityUserToken<string>, IEntity
{
    [DisplayName("GeçerlilikTarihi")]
    public DateTime ExpireDate { get; set; }
}