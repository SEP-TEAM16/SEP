using SEP.Common.Models;
using SEP.Common.Enums;

namespace SEP.Gateway.Models
{
    public class Endpoint : Entity
    {
        public EndpointType EndpointType { get; set; }
        public string URL { get; set; }
    }
}
