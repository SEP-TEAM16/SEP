namespace SEP.WebShop.Web.Dto
{
    public class PaymentDto
    {
        public PaymentDto() { }
        public float Amount { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string ItemName { get; set; }
        public string Currency { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public MerchantDto Merchant { get; set; }
        public string Key { get; set; }
        public string IdentityToken { get; set; }

        public PaymentDto(float amount, string name, string email, string description, string itemName, string currency, string firstName, string lastName, MerchantDto merchant, string key, string identityToken)
        {
            Amount = amount;
            Name = name;
            Email = email;
            Description = description;
            ItemName = itemName;
            Currency = currency;
            FirstName = firstName;
            LastName = lastName;
            Merchant = merchant;
            Key = key;
            IdentityToken = identityToken;
        }
    }
}
