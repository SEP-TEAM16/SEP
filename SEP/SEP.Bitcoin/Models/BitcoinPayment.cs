using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.Bitcoin.Models
{
    public class BitcoinPayment : Payment
    {
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

        public BitcoinPayment() : base() { }

        public BitcoinPayment(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string identityToken, string merchantId, string publicKey, string privateKey)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval, identityToken)
        {
            MerchantId = merchantId;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public BitcoinPayment(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string identityToken, string merchantId)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval, identityToken)
        {
            MerchantId = merchantId;
        }
    }
}
