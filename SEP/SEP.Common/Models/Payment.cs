using SEP.Common.Enums;

namespace SEP.Common.Models
{
    public class Payment : Entity
    {
        public Payment(float amount, string name, string firstName, string lastName, string email, DateTime date, string currency, string description, string itemName, PaymentApprovalType paymentApproval)
        {
            Amount = amount;
            Name = name;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Date = date;
            Currency = currency;
            Description = description;
            ItemName = itemName;
            PaymentApproval = paymentApproval;
        }
        public Payment() { }
        public float Amount { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string ItemName { get; set; }
        public PaymentApprovalType PaymentApproval { get; set; }

    }
}
