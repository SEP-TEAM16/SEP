namespace SEP.PSP.Models
{
    public class AuthTokenDTO
    {
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public AuthTokenDTO() { }
        public AuthTokenDTO(string token, DateTime expirationDate)
        {
            Token = token;
            ExpirationDate = expirationDate;
        }
    }
}
