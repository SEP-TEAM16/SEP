using SEP.Common.Enums;
using SEP.Common.Models;
using SEP.PSP.Models;

namespace SEP.PSP.DTO
{
    public class PSPBankPaymentDTO : Payment
    {
        public string MerchantId { get; set; }

        public PSPBankPaymentDTO() : base() { }
        public PSPBankPaymentDTO(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string identityToken, string merchantId)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval, identityToken)
        {
            MerchantId = merchantId;
        }

        public PSPPayment ConvertToPSPPayment()
        {
            var merchant = new Merchant
            {
                Id = MerchantId is not null ? int.Parse(MerchantId) : 0
            };
            return new PSPPayment(Amount, Name, FirstName, LastName, Email, Date, Currency, Description, ItemName, PaymentApproval, IdentityToken, merchant);
        }
    }
}
