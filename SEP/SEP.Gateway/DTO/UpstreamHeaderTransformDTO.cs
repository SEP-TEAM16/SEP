namespace SEP.Gateway.DTO
{
    public class UpstreamHeaderTransformDTO
    {
       public string SenderPort { get; set; }

        public UpstreamHeaderTransformDTO(string senderPort)
        {
            SenderPort = senderPort;
        }
    }
}
