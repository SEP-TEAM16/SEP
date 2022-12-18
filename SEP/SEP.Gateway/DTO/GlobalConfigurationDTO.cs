namespace SEP.Gateway.DTO
{
    public class GlobalConfigurationDTO
    {
        public string BaseUrl { get; set; }
        public GlobalConfigurationDTO(string baseUrl)
        {
            BaseUrl = baseUrl;
        }


    }
}
