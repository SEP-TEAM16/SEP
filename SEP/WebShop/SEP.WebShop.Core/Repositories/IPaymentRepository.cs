using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities;

namespace SEP.WebShop.Core.Repositories
{
    public interface IPaymentRepository : IRepository<Payment, Guid>
    {
        Maybe<Payment> FindByIdentityToken(string identityToken);
    }
}
