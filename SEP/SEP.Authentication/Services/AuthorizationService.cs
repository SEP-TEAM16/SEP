using SEP.Autorization.Infrastructure;
using SEP.Autorization.Interfaces;
using SEP.Common.Models;

namespace SEP.Autorization.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly AuthorizationDbContext _autorizationDbContext;
        public AuthorizationService(AuthorizationDbContext autorizationDbContext)
        {
            _autorizationDbContext = autorizationDbContext;
            autorizationDbContext.AuthKeys.RemoveRange(_autorizationDbContext.AuthKeys.ToList());
            autorizationDbContext.SaveChanges();
        }

        public List<AuthKey> GetAuthKeys()
        {
            return _autorizationDbContext.AuthKeys.ToList();
        }
        public bool AddAuthKey(AuthKey authKey)
        {
            if (_autorizationDbContext.AuthKeys.FirstOrDefault(key => key.Route.Equals(authKey.Route)) is not null)
                return false;

            _autorizationDbContext.AuthKeys.Add(authKey);
            _autorizationDbContext.SaveChanges();
            return true;
        }
    }
}
