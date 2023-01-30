using SEP.Common.Enums;
using SEP.Common.Models;
using SEP.PSP.Models;

namespace SEP.PSP.DTO
{
    public class PSPBitcoinPaymentDTOForReceive : Payment
    {
        public Merchant Merchant { get; set; }
        public string Key { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }


        public PSPBitcoinPaymentDTOForReceive() : base() { }
        public PSPBitcoinPaymentDTOForReceive(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string identityToken, Merchant merchant, string publicKey, string privateKey)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval, identityToken)
        {
            Merchant = merchant;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public PSPBitcoinPaymentDTOForReceive(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string identityToken, Merchant merchant)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval, identityToken)
        {
            Merchant = merchant;
        }
    }
}
