using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities.Enums;
using SEP.WebShop.Core.Entities.ValueObjects;

namespace SEP.WebShop.Core.Entities
{
    public class SubscriptionOption
    {
        public Guid Id { get; }
        public SubscriptionType SubscriptionType { get; }
        public Name Name { get; }
        public Price Price { get; }
        public Currency Currency { get; }
        
        private SubscriptionOption(Guid id, SubscriptionType subscriptionType, Name name, Price price, Currency currency)
        {
            Id = id;
            SubscriptionType = subscriptionType;
            Name = name;
            Price = price;
            Currency = currency;
        }

        public static Result<SubscriptionOption> Create(Guid id, SubscriptionType subscriptionType, string name, double price, string currency)
        {
            Result<Name> nameResult = Name.Create(name);
            Result<Price> priceResult = Price.Create(price);
            Result<Currency> currencyResult = Currency.Create(currency);
            if (Result.Combine(nameResult, priceResult, currencyResult).IsFailure)
            {
                return Result.Failure<SubscriptionOption>("Subscription option creating failed because of invalid parameters");
            }
            return Result.Success(new SubscriptionOption(id, subscriptionType, nameResult.Value, priceResult.Value, currencyResult.Value));
        }

        public SubscriptionOption Update(SubscriptionOption subscriptionOption) => Create(subscriptionOption.Id, subscriptionOption.SubscriptionType, subscriptionOption.Name, subscriptionOption.Price, subscriptionOption.Currency).Value;
    }
}
