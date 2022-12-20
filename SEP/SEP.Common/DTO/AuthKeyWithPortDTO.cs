using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.Common.DTO
{
    public class AuthKeyWithPortDTO : AuthKey
    {
        public int Port { get; set; }
        public bool NeedAuth { get; set; }

        public AuthKeyWithPortDTO() : base() { }
        public AuthKeyWithPortDTO(string key, string route, int port, bool needAuth, string type, int paymentMicroserviceType) : base(key, route, type, 0, paymentMicroserviceType)
        {
            Port = port;
            NeedAuth = needAuth;
        }
    }
}
