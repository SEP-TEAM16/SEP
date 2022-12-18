namespace SEP.WebShop.Web.Dto
{
    public class SubscriptionDto
    {
        public Guid Id { get; }
        public DateTime ExpirationDateTime { get; }
        public string SubscriptionOptionName { get; }
        public string SubscriptionOptionType { get; }
        public string CompanyName { get; }

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
