using SEP.Common.Models;

namespace SEP.PSP.Models
{
    public class Merchant : Entity
    {
        public string Port { get; set; }
        public string Key { get; set; }
        public Merchant() { }
        public Merchant(string port, string key)
        {
            Port = port;
            Key = key;
        }
    }
}
