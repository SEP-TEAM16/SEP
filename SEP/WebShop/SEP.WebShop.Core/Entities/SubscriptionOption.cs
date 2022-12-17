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
        
        private SubscriptionOption(Guid id, SubscriptionType subscriptionType, Name name)
        {
            Id = id;
            SubscriptionType = subscriptionType;
            Name = name;
        }

        public static Result<SubscriptionOption> Create(Guid id, SubscriptionType subscriptionType, string name)
        {
            Result<Name> nameResult = Name.Create(name);
            if (nameResult.IsFailure)
            {
                return Result.Failure<SubscriptionOption>("Subscription option creating failed because of invalid parameters");
            }
            return Result.Success(new SubscriptionOption(id, subscriptionType, nameResult.Value));
        }

        public SubscriptionOption Update(SubscriptionOption subscriptionOption) => Create(subscriptionOption.Id, subscriptionOption.SubscriptionType, subscriptionOption.Name).Value;
    }
}
