using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.Bitcoin.DTO
{
    public class BitcoinPaymentDTO : Payment
    {
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

        public BitcoinPaymentDTO() : base() { }

        public BitcoinPaymentDTO(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string identityToken, string merchantId, string publicKey, string privateKey)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval, identityToken)
        {
            MerchantId = merchantId;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public BitcoinPaymentDTO(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            int paymentApproval, string identityToken, string merchantId, string publicKey, string privateKey)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, (PaymentApprovalType) paymentApproval, identityToken)
        {
            MerchantId = merchantId;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }
    }
}
