﻿using SEP.Common.Models;

namespace SEP.WebShop.Web.Dto
{
    public class PaymentBitcoinDto
    {
        public PaymentBitcoinDto() { }
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
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }

        public PaymentBitcoinDto(float amount, string name, string email, string description, string itemName, string currency, string firstName, string lastName, MerchantDto merchant, string key, string identityToken, string privateKey, string publicKey)
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
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }
    
}
}
