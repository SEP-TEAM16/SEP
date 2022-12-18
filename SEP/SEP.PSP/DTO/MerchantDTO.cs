using SEP.Common.Models;

namespace SEP.PSP.DTO
{
    public class MerchantDTO : Entity
    {
        public string Port { get; set; }
        public string Key { get; set; }
        public MerchantDTO() { }
        public MerchantDTO(string port, string key)
        {
            Port = port;
            Key = key;
        }
    }
}
