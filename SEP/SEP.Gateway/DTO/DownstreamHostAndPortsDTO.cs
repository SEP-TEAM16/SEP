namespace SEP.Gateway.DTO
{
    public class DownstreamHostAndPortsDTO
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public DownstreamHostAndPortsDTO(string host, int port)
        {
            Host = host;
            Port = port;
        }
    }
}
