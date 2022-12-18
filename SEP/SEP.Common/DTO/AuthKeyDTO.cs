using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.Common.DTO
{
    public class AuthKeyDTO : AuthKey
    {
        public AuthKeyDTO(string key, string route, string type, PaymentMicroserviceType paymentMicroserviceType) : base(key, route, type, 0, paymentMicroserviceType)
        {
        }
    }
}
