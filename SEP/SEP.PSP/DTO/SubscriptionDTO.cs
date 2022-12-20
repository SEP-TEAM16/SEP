using SEP.Common.Enums;
using SEP.Common.Models;
using SEP.PSP.Models;

namespace SEP.PSP.DTO
{
    public class SubscriptionDTO : Entity
    {
        public Merchant Merchant { get; set; }
        public PaymentMicroserviceType PaymentMicroserviceType { get; set; }

        public SubscriptionDTO() { }
        public SubscriptionDTO(PaymentMicroserviceType paymentMicroserviceType)
        {
            PaymentMicroserviceType = paymentMicroserviceType;
        }
        public SubscriptionDTO(Merchant merchant, PaymentMicroserviceType paymentMicroserviceType)
        {
            Merchant = merchant;
            PaymentMicroserviceType = paymentMicroserviceType;
        }
    }
}
