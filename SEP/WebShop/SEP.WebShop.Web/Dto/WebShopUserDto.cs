using SEP.WebShop.Core.Entities.Enums;

namespace SEP.WebShop.Web.Dto
{
    public class WebShopUserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public UserType UserType { get; set; }

        public WebShopUserDto() { }

        public WebShopUserDto(Guid id, string username, string password, string emailAddress, string name, string city, string street, UserType userType)
        {
            Id = id;
            Username = username;
            Password = password;
            EmailAddress = emailAddress;
            Name = name;
            City = city;
            Street = street;
            UserType = userType;
        }
    }
}
