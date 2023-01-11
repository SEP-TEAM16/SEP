using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.Bitcoin.DTO
{
    public class BitcoinPaymentWithoutKeysDTO : Payment
    {
        public string MerchantId { get; set; }

        public BitcoinPaymentWithoutKeysDTO() : base() { }

        public BitcoinPaymentWithoutKeysDTO(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string identityToken, string merchantId)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval, identityToken)
        {
            MerchantId = merchantId;
        }

        public BitcoinPaymentWithoutKeysDTO(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            int paymentApproval, string identityToken, string merchantId)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, (PaymentApprovalType) paymentApproval, identityToken)
        {
            MerchantId = merchantId;
        }
    }
}
