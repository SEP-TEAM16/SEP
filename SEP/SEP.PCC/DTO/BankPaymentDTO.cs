using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.PCC.DTO
{
    public class BankPaymentDTO : Payment
    {
        public string MerchantId { get; set; }
        public DateTime Expiration { get; set; }
        public string Number { get; set; }
        public string SecurityCode { get; set; }

        public BankPaymentDTO() : base() { }

        public BankPaymentDTO(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, string identityToken, string merchantId, DateTime expiration, string number, string securityCode)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval, identityToken)
        {
            MerchantId = merchantId;
            Expiration = expiration;
            Number = number;
            SecurityCode = securityCode;
        }

        public BankPaymentDTO(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            int paymentApproval, string identityToken, string merchantId, DateTime expiration, string number, string securityCode)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, (PaymentApprovalType) paymentApproval, identityToken)
        {
            MerchantId = merchantId;
            Expiration = expiration;
            Number = number;
            SecurityCode = securityCode;
        }
    }
}
