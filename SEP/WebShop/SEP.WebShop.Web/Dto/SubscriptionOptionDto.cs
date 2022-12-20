using SEP.WebShop.Core.Entities.Enums;

namespace SEP.WebShop.Web.Dto
{
    public class SubscriptionOptionDto
    {
        public Guid Id { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }

        public SubscriptionOptionDto()
        {

        }

        public SubscriptionOptionDto(Guid id, SubscriptionType subscriptionType, string name, double price, string currency)
        {
            Id = id;
            SubscriptionType = subscriptionType;
            Name = name;
            Price = price;
            Currency = currency;
        }
       
    }
}
