using SEP.Common.Models;

namespace SEP.Common.DTO
{
    public class AuthKeyWithKeyDTO : AuthKey
    {
        public string KeyForAutorization { get; set; }

        public AuthKeyWithKeyDTO(string key, string route, string keyForAutorization) : base(key, route)
        {
            KeyForAutorization = keyForAutorization;
        }
    }
}
