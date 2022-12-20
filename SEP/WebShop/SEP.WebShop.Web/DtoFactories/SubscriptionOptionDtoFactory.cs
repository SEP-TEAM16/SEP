using SEP.WebShop.Core.Entities;
using SEP.WebShop.Web.Dto;

namespace SEP.WebShop.Web.DtoFactories
{
    public class SubscriptionOptionDtoFactory
    {
        public SubscriptionOptionDto Create(SubscriptionOption option)
        {
            return new SubscriptionOptionDto(option.Id, option.SubscriptionType, option.Name, option.Price, option.Currency);
        }
    }
}
