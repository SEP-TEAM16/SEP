using CSharpFunctionalExtensions;
using SEP.WebShop.Core.Entities.Enums;

namespace SEP.WebShop.Core.Entities
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public DateTime ExpirationDateTime { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
        public SubscriptionOption SubscriptionOption { get; set; }
        public Company Company { get; set; }

        public Subscription() { }
        
        private Subscription(Guid id, DateTime expirationDateTime, SubscriptionStatus subscriptionStatus, SubscriptionOption subscriptionOption, Company company)
        {
            Id = id;
            ExpirationDateTime = expirationDateTime;
            SubscriptionOption = subscriptionOption;
            Company = company;
            SubscriptionStatus = subscriptionStatus;
        }


        public static Result<Subscription> Create(Guid id, DateTime expirationDateTime, SubscriptionStatus subscriptionStatus, SubscriptionOption subscriptionOption, Company company)
        {
            return Result.Success(new Subscription(id, expirationDateTime, subscriptionStatus, subscriptionOption, company));
        }

        public Subscription Update(Subscription subscription) => Create(subscription.Id, subscription.ExpirationDateTime, subscription.SubscriptionStatus, subscription.SubscriptionOption, subscription.Company).Value;
    }
}
