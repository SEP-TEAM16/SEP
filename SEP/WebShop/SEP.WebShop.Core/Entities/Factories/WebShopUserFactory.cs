using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities.Enums;

namespace SEP.WebShop.Core.Entities.Factories
{
    public class WebShopUserFactory
    {
        public Result<WebShopUser> Create(Guid id, string username, string password, string emailAddress, string name, string city, string street, UserType userType)
        {
            if (userType == UserType.candidate)
            {
                Result<Candidate> result = Candidate.Create(id, username, password, emailAddress, name, city, street, userType);
                if (result.IsFailure)
                {
                    return Result.Failure<WebShopUser>("Unable to create candidate because of invalid parameters");
                }
                return Result.Success<WebShopUser>(result.Value);
            }
            else
            {
                Result<Company> result = Company.Create(id, username, password, emailAddress, name, city, street, userType);
                if (result.IsFailure)
                {
                    return Result.Failure<WebShopUser>("Unable to create company because of invalid parameters");
                }
                return Result.Success<WebShopUser>(result.Value);
            }
        }

        public Result<WebShopUser> Create(Guid id, string username, string emailAddress, string name, string city, string street, UserType userType)
        {
            if (userType == UserType.candidate)
            {
                Result<Candidate> result = Candidate.Create(id, username, emailAddress, name, city, street, userType);
                if (result.IsFailure)
                {
                    return Result.Failure<WebShopUser>("Unable to create candidate because of invalid parameters");
                }
                return Result.Success<WebShopUser>(result.Value);
            }
            else
            {
                Result<Company> result = Company.Create(id, username, emailAddress, name, city, street, userType);
                if (result.IsFailure)
                {
                    return Result.Failure<WebShopUser>("Unable to create company because of invalid parameters");
                }
                return Result.Success<WebShopUser>(result.Value);
            }
        }
    }
}
