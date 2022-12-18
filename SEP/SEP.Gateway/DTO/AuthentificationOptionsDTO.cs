namespace SEP.Gateway.DTO
{
    public class AuthentificationOptionsDTO
    {
        public string AuthenticationProviderKey { get; set; }
        public List<string> AllowedScopes { get; set; }

        public AuthentificationOptionsDTO(string authenticationProviderKey, List<string> allowedScopes)
        {
            AuthenticationProviderKey = authenticationProviderKey;
            AllowedScopes = allowedScopes;
        }
    }
}
