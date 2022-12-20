namespace SEP.WebShop.Web.Dto
{
    public class MerchantDto
    {
        public MerchantDto() { }
        public int Id { get; set; }
        public string Key { get; set; }
        public string Port { get; set; }

        public MerchantDto(int id, string key, string port)
        {
            Id = id;
            Key = key;
            Port = port;
        }
    }
}
