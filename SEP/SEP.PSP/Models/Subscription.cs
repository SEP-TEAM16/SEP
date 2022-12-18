using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.PSP.Models
{
    public class Subscription : Entity
    {
        public Merchant Merchant { get; set; }
        public PaymentMicroserviceType PaymentMicroserviceType { get; set; }
        public Subscription() { }
        public Subscription(Merchant merchant, PaymentMicroserviceType paymentMicroserviceType)
        {
            Merchant = merchant;
            PaymentMicroserviceType = paymentMicroserviceType;
        }
    }
}
