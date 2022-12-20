using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.PayPal.Models
{
    public class PayPalPayment : Payment
    {
        public string MerchantId { get; set; }
        public string Token { get; set; }

        public PayPalPayment() : base() { }

        public PayPalPayment(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string identityToken, string merchantId, string token)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval, identityToken)
        {
            MerchantId = merchantId;
            Token = token;
        }
    }
}
