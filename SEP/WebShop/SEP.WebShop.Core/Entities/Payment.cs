using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities.Enums;
using SEP.WebShop.Core.Entities.ValueObjects;

namespace SEP.WebShop.Core.Entities
{
    public class Payment
    {
        public Guid Id { get; }
        public Name ItemName { get; }
        public Price Price { get; }
        public Currency Currency { get; }
        public Guid SubscriptionId { get; }
        public PaymentStatus PaymentStatus { get; }

        private Payment(Guid id, Name itemName, Price price, Currency currency, Guid subscriptionId, PaymentStatus paymentStatus)
        {
            Id = id;
            ItemName = itemName;
            Price = price;
            Currency = currency;
            SubscriptionId = subscriptionId;
            PaymentStatus = paymentStatus;
        }


        public static Result<Payment> Create(Guid id, string name, double price, string currency, Guid subscriptionId, PaymentStatus paymentStatus)
        {
            Result<Name> nameResult = Name.Create(name);
            Result<Price> priceResult = Price.Create(price);
            Result<Currency> currencyResult = Currency.Create(currency);
            if(Result.Combine(nameResult, priceResult, currencyResult).IsFailure)
            {
                return Result.Failure<Payment>("Payment creating failed because of invalid parameters");
            }
            return Result.Success(new Payment(id, nameResult.Value, priceResult.Value, currencyResult.Value, subscriptionId, paymentStatus));
        }

        public Payment Update(Payment payment) => Create(Id, payment.ItemName, payment.Price, payment.Currency, payment.SubscriptionId, payment.PaymentStatus).Value;
    }
}
