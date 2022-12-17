using SEP.WebShop.Core.Entities;
using SEP.WebShop.Web.Dto;

namespace SEP.WebShop.Web.DtoFactories
{
    public class WebShopUserDtoFactory
    {
        public WebShopUserDto Create(WebShopUser user)
        {
            return new WebShopUserDto(user.Id, user.Username, user.EmailAddress, user.Password, user.Name, user.Address.City, user.Address.Street, user.UserType);
        }
    }
}
