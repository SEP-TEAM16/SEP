using SEP.Common.Enums;
using SEP.Common.Models;
using SEP.PSP.DTO;

namespace SEP.PSP.Models
{
    public class PSPPayment : Payment
    {
        public Merchant Merchant { get; set; }

        PSPPayment() { }
        public PSPPayment(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string identityToken, Merchant merchant)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval, identityToken)
        {
            Merchant = merchant;
        }

        public PSPPayPalPaymentDTO ConvertToPSPPayPalPaymentDTO()
        {
            var merchantId = Merchant.Id.ToString();
            return new PSPPayPalPaymentDTO(Amount, Name, FirstName, LastName, Email, Date, Currency, Description, ItemName, PaymentApproval, IdentityToken, merchantId);
        }

        public PSPPayPalPaymentDTO ConvertToPSPBankPaymentDTO()
        {
            var merchantId = Merchant.Id.ToString();
            return new PSPPayPalPaymentDTO(Amount, Name, FirstName, LastName, Email, Date, Currency, Description, ItemName, PaymentApproval, IdentityToken, merchantId);
        }
    }
}
