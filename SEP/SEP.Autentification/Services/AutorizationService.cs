using SEP.Autorization.Infrastructure;
using SEP.Autorization.Interfaces;
using SEP.Common.Models;

namespace SEP.Autorization.Services
{
    public class AutorizationService : IAutorizationService
    {
        private readonly AutorizationDbContext autorizationDbContext;
        public AutorizationService(AutorizationDbContext autorizationDbContext)
        {
            this.autorizationDbContext = autorizationDbContext;
        }

        public List<AuthKey> GetAuthKeys()
        {
            return autorizationDbContext.AuthKeys.ToList();
        }
        public void AddAuthKey(AuthKey authKey)
        {
            autorizationDbContext.AuthKeys.Add(authKey);
        }
    }
}
