using SEP.Common.Enums;
using SEP.Common.Models;
using SEP.PSP.Models;

namespace SEP.PSP.DTO
{
    public class PSPBitcoinPaymentDTO : Payment
    {
        public string MerchantId { get; set; }
        public string Key { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }


        public PSPBitcoinPaymentDTO() : base() { }
        public PSPBitcoinPaymentDTO(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string identityToken, string merchantId, string publicKey, string privateKey)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval, identityToken)
        {
            MerchantId = merchantId;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public PSPBitcoinPaymentDTO(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string identityToken, string merchantId)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval, identityToken)
        {
            MerchantId = merchantId;
        }
    }
}
