using SEP.Common.Models;

namespace SEP.Autorization.Interfaces
{
    public interface IAuthorizationService
    {
        public List<AuthKey> GetAuthKeys();
        public bool AddAuthKey(AuthKey authKey);
    }
}
