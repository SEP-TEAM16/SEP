using SEP.Common.Enums;
using SEP.Common.Models;
using SEP.PSP.Models;

namespace SEP.PSP.DTO
{
    public class PSPPaymentDTO : Payment
    {
        public Merchant Merchant { get; set; }
        public string Key { get; set; }
        PSPPaymentDTO() { }
        public PSPPaymentDTO(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, Merchant merchant)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval)
        {
            Merchant = merchant;
        }
    }
}
