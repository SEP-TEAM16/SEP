using SEP.WebShop.Models;

namespace SEP.Webshop.Models
{
    public class Company : User
    {
        public string Name { get; set; }
        public int PIB { get; set; }
        public DateTime DateOfEstablishment { get; set; }

    }
}
