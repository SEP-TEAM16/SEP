using SEP.Common.Models;

namespace SEP.Autorization.Interfaces
{
    public interface IAutorizationService
    {
        public List<AuthKey> GetAuthKeys();
        public void AddAuthKey(AuthKey authKey);
    }
}
