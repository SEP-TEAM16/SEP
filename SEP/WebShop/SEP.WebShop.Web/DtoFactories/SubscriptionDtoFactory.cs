using SEP.WebShop.Core.Entities;
using SEP.WebShop.Web.Dto;

namespace SEP.WebShop.Web.DtoFactories
{
    public class SubscriptionDtoFactory
    {
        public SubscriptionDto Create(Subscription subscription)
        {
            return new SubscriptionDto(subscription.Id, subscription.ExpirationDateTime, subscription.SubscriptionOption.Name, subscription.SubscriptionOption.SubscriptionType.ToString(), subscription.Company.Name);
        }
    }
}
