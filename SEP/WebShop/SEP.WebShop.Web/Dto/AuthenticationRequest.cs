namespace SEP.WebShop.Web.Dto
{
    public class AuthenticationRequest
    {

        public string Username { get; set; }
        public string Password { get; set; }

        public AuthenticationRequest() { }

        public AuthenticationRequest(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
