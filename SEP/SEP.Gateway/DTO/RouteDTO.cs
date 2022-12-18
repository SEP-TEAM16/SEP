namespace SEP.Gateway.DTO
{
    public class RouteDTO
    {
        public string UpstreamPathTemplate { get; set; }
        public List<string>  UpstreamHttpMethod { get; set; }
        public string  DownstreamPathTemplate { get; set; }
        public string DownstreamScheme { get; set; }
        public UpstreamHeaderTransformDTO UpstreamHeaderTransform { get; set; }
        public AuthentificationOptionsDTO AuthenticationOptions { get; set; }
        public List<DownstreamHostAndPortsDTO> DownstreamHostAndPorts { get; set; }

        public RouteDTO(string upstreamPathTemplate, List<string> upstreamHttpMethod, string downstreamPathTemplate, string downstreamScheme, AuthentificationOptionsDTO authentificationOptions, List<DownstreamHostAndPortsDTO> downstreamHostAndPorts, UpstreamHeaderTransformDTO upstreamHeaderTransform)
        {
            UpstreamPathTemplate = upstreamPathTemplate;
            UpstreamHttpMethod = upstreamHttpMethod;
            DownstreamPathTemplate = downstreamPathTemplate;
            DownstreamScheme = downstreamScheme;
            AuthenticationOptions = authentificationOptions;
            DownstreamHostAndPorts = downstreamHostAndPorts;
            UpstreamHeaderTransform = upstreamHeaderTransform;
        }

        public RouteDTO(string upstreamPathTemplate, List<string> upstreamHttpMethod, string downstreamPathTemplate, string downstreamScheme, List<DownstreamHostAndPortsDTO> downstreamHostAndPorts)
        {
            UpstreamPathTemplate = upstreamPathTemplate;
            UpstreamHttpMethod = upstreamHttpMethod;
            DownstreamPathTemplate = downstreamPathTemplate;
            DownstreamScheme = downstreamScheme;
            DownstreamHostAndPorts = downstreamHostAndPorts;
        }
    }
}
