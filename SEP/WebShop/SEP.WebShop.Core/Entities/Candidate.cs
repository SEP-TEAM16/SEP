using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities.ValueObjects;
using SEP.WebShop.Core.Entities.Enums;

namespace SEP.WebShop.Core.Entities
{
    public class Candidate : WebShopUser
    {

        private Candidate(Guid id, Username username, HashedPassword password, EmailAddress emailAddress, Name name, Address address, UserType userType) : base(id, username, password, emailAddress, name, address, userType)
        {
        }

        private Candidate(Guid id, Username username, EmailAddress emailAddress, Name name, Address address, UserType userType) : base(id, username, emailAddress, name, address, userType)
        {
        }

        public static Result<Candidate> Create(Guid id, string username, string password, string emailAddress, string name, string city, string street, UserType userType)
        {
            Result<Username> usernameResult = Username.Create(username);
            Result<HashedPassword> passwordResult = HashedPassword.Create(password);
            Result<EmailAddress> emailAddressResult = EmailAddress.Create(emailAddress);
            Result<Name> nameResult = Name.Create(name);
            Result<Address> addressResult = Address.Create(city, street);
            Result result = Result.Combine(usernameResult, passwordResult, emailAddressResult, nameResult, addressResult);
            if (result.IsFailure)
            {
                return Result.Failure<Candidate>("Candidate creating failed because of invalid parameters");
            }
            return Result.Success(new Candidate(id, usernameResult.Value, passwordResult.Value, emailAddressResult.Value, nameResult.Value, addressResult.Value, userType));
        }

        public static Result<Candidate> Create(Guid id, string username, string emailAddress, string name, string city, string street, UserType userType)
        {
            Result<Username> usernameResult = Username.Create(username);
            Result<EmailAddress> emailAddressResult = EmailAddress.Create(emailAddress);
            Result<Name> nameResult = Name.Create(name);
            Result<Address> addressResult = Address.Create(city, street);
            Result result = Result.Combine(usernameResult, emailAddressResult, nameResult, addressResult);
            if (result.IsFailure)
            {
                return Result.Failure<Candidate>("Candidate creating failed because of invalid parameters");
            }
            return Result.Success(new Candidate(id, usernameResult.Value, emailAddressResult.Value, nameResult.Value, addressResult.Value, userType));
        }

        public override WebShopUser Update(WebShopUser webShopUser) => Create(Id, webShopUser.Username, Password, webShopUser.EmailAddress, webShopUser.Name, webShopUser.Address.City, webShopUser.Address.Street, webShopUser.UserType).Value;

    }
}
