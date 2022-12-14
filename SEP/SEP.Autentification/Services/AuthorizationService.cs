using SEP.Autorization.Infrastructure;
using SEP.Autorization.Interfaces;
using SEP.Common.Models;

namespace SEP.Autorization.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly AuthorizationDbContext autorizationDbContext;
        public AuthorizationService(AuthorizationDbContext autorizationDbContext)
        {
            this.autorizationDbContext = autorizationDbContext;
        }

        public List<AuthKey> GetAuthKeys()
        {
            return autorizationDbContext.AuthKeys.ToList();
        }
        public bool AddAuthKey(AuthKey authKey)
        {
            if (autorizationDbContext.AuthKeys.FirstOrDefault(key => key.Route.Equals(authKey.Route)) is not null)
                return false;

            autorizationDbContext.AuthKeys.Add(authKey);
            autorizationDbContext.SaveChanges();
            return true;

        }
    }
}
