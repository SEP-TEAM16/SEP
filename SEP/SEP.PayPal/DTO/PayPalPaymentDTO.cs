using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.PayPal.DTO
{
    public class PayPalPaymentDTO : Payment
    {
        public string MerchantID { get; set; }

        PayPalPaymentDTO() { }

        public PayPalPaymentDTO(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string merchantID)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval)
        {
            MerchantID = merchantID;
        }
    }
}
