using SEP.WebShop.Core.Entities.Enums;

namespace SEP.WebShop.Web.Dto
{
    public class SubscriptionOptionDto
    {
        public Guid Id { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string Name { get; set; }

        public SubscriptionOptionDto()
        {

        }

        public SubscriptionOptionDto(Guid id, SubscriptionType subscriptionType, string name)
        {
            Id = id;
            SubscriptionType = subscriptionType;
            Name = name;
        }
       
    }
}
