namespace SEP.WebShop.Web.Dto
{
    public class SubscriptionDto
    {
        public Guid Id { get; set; }
        public DateTime ExpirationDateTime { get; set;  }
        public string SubscriptionOptionName { get; set;  }
        public string SubscriptionOptionType { get; set;  }
        public string CompanyName { get; set; }

        public SubscriptionDto()
        {

        }

        public SubscriptionDto(Guid id, DateTime expirationDateTime, string subscriptionOptionName, string subscriptionOptionType, string companyName)
        {
            Id = id;
            ExpirationDateTime = expirationDateTime;
            SubscriptionOptionName = subscriptionOptionName;
            SubscriptionOptionType = subscriptionOptionType;
            CompanyName = companyName;  
        }
    }
}
