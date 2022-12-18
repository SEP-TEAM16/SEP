using SEP.Common.Models;

namespace SEP.Common.DTO
{
    public class AuthKeyWithPortDTO : AuthKey
    {
        public int Port { get; set; }
        public bool NeedAuth { get; set; }

        public AuthKeyWithPortDTO(string key, string route, int port, bool needAuth, string type) : base(key, route, type, 0)
        {
            Port = port;
            NeedAuth = needAuth;
        }
    }
}
