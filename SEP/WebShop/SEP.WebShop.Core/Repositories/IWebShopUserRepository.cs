using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;

namespace SEP.WebShop.Core.Repositories
{
    public interface IWebShopUserRepository : IRepository<WebShopUser, Guid>
    {
        Maybe<WebShopUser> FindByUsername(string username);
    }
}
