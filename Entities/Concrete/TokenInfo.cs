using Core.Entities;

namespace Entities.Concrete;

public class TokenInfo : IEntity
{
    public string Token { get; set; } = null!;
    public DateTime ExpireDate { get; set; }
}