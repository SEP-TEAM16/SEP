using CSharpFunctionalExtensions;

namespace SEP.WebShop.Core.Entities
{
    public class Subscription
    {
        public Guid Id { get; }
        public DateTime ExpirationDateTime { get; }
        public SubscriptionOption SubscriptionOption { get; }
        public Company Company { get; }
        
        private Subscription(Guid id, DateTime expirationDateTime, SubscriptionOption subscriptionOption, Company company)
        {
            Id = id;
            ExpirationDateTime = expirationDateTime;
            SubscriptionOption = subscriptionOption;
            Company = company;  
        }


        public static Result<Subscription> Create(Guid id, DateTime expirationDateTime, SubscriptionOption subscriptionOption, Company company)
        {
            return Result.Success(new Subscription(id, expirationDateTime, subscriptionOption, company));
        }

        public Subscription Update(Subscription subscription) => Create(subscription.Id, subscription.ExpirationDateTime, subscription.SubscriptionOption, subscription.Company).Value;
    }
}
