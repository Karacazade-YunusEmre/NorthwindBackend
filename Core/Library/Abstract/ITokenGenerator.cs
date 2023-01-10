using Core.Entities.Concrete.Authentication;
using Microsoft.EntityFrameworkCore;

namespace Core.Library.Abstract;

public interface ITokenGenerator
{
    public Task<UserToken> GetToken<TContext>(User user) where TContext : DbContext, new();
    public Task<bool> DeleteToken<TContext>(User user) where TContext : DbContext, new();
}