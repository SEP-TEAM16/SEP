namespace SEP.Common.Models
{
    public class ServiceSecret : Entity
    {
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
