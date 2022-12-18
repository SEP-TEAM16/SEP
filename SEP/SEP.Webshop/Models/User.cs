using SEP.Common.Models;

namespace SEP.WebShop.Models
{
    public class User : Entity
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
