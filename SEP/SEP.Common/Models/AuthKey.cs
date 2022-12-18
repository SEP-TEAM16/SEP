using SEP.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SEP.Common.Models
{
    public class AuthKey : Entity
    {
        public string Key { get; set; }
        public string Route { get; set; }
        public string Type { get; set; }
        public PaymentMicroserviceType PaymentMicroserviceType { get; set; }

        public AuthKey(string key, string route, string type, int id, PaymentMicroserviceType paymentMicroserviceType)
        {
            Id = id;
            Key = key;
            Route = route;
            Type = type;
            PaymentMicroserviceType = paymentMicroserviceType;
        }
    }
}
