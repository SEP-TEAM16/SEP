using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;

namespace SEP.WebShop.Web.Authorization
{
    public interface IJwtUtils
    {
        public string GenerateToken(WebShopUser user);
        public Maybe<Guid> ValidateToken(string token);
    }
}
