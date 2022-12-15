using SEP.Common.Models;

namespace SEP.Common.DTO
{
    public class AuthKeyWithKeyDTO : AuthKey
    {
        public string KeyForAutorization { get; set; }

        public AuthKeyWithKeyDTO(string key, string route, string keyForAutorization, string type) : base(key, route, type, 0)
        {
            KeyForAutorization = keyForAutorization;
        }
    }
}
