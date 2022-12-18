using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.PayPal.Models
{
    public class PayPalPayment : Payment
    {
        public string MerchantID { get; set; }
        public string Token { get; set; }

        PayPalPayment() { }

        public PayPalPayment(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string merchantID, string token)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval)
        {
            MerchantID = merchantID;
            Token = token;
        }
    }
}
