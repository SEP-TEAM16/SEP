namespace SEP.WebShop.Web.Dto
{
    public class PackageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Currency { get; set; }
        public double Price { get; set; }

        public PackageDto() { }

        public PackageDto(Guid id, string name, string currency, double price)
        {
            Id = id;
            Name = name;
            Currency = currency;
            Price = price;
        }
    }
}
