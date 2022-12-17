using SEP.WebShop.Core.Entities.Enums;
using SEP.WebShop.Core.Entities.ValueObjects;

namespace SEP.WebShop.Core.Entities
{
    public abstract class WebShopUser : User
    {
        public Name Name { get; }
        public Address Address { get; }
        public UserType UserType { get; }

        protected WebShopUser(Guid id, Username username, HashedPassword password, EmailAddress emailAddress, Name name, Address address, UserType userType) : base(id, username, password, emailAddress)
        {
            Name = name;
            Address = address;
            UserType = userType;
        }

        protected WebShopUser(Guid id, Username username, EmailAddress emailAddress, Name name, Address address, UserType userType) : base(id, username, emailAddress)
        {
            Name = name;
            Address = address;
            UserType = userType;
        }


        public abstract WebShopUser Update(WebShopUser webShopUser);
    }
}
