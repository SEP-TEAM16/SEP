using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.PSP.Models
{
    public class PSPPayment : Payment
    {
        public Merchant Merchant { get; set; }
        PSPPayment() { }
        public PSPPayment(float amount, string name, string firstName, string lastName,
            string email, DateTime date, string currency, string description, string itemName,
            PaymentApprovalType paymentApproval, Merchant merchant)
            : base(amount, name, firstName, lastName, email, date, currency, description, itemName, paymentApproval)
        {
            Merchant = merchant;
        }
    }
}
