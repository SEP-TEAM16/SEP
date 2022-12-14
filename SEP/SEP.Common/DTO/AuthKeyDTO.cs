using SEP.Common.Models;

namespace SEP.Common.DTO
{
    public class AuthKeyDTO : AuthKey
    {
        public AuthKeyDTO(string key, string route) : base(key, route)
        {
        }
    }
}
