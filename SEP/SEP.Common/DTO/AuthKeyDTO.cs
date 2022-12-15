using SEP.Common.Models;

namespace SEP.Common.DTO
{
    public class AuthKeyDTO : AuthKey
    {
        public AuthKeyDTO(string key, string route, string type) : base(key, route, type, 0)
        {
        }
    }
}
