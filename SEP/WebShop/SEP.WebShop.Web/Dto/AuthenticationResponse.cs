using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Entities.Enums;

namespace SEP.WebShop.Web.Dto
{
    public class AuthenticationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public UserType UserType { get; set; }

        public AuthenticationResponse() { }

        public AuthenticationResponse(WebShopUser user, string token)
        {
            Id = user.Id;
            Name = user.Name;
            Username = user.Username;
            Token = token;
            UserType = user.UserType;
        }
    }
}
