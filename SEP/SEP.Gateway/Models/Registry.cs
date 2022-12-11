using SEP.Common.Models;

namespace SEP.Gateway.Models
{
    public class Registry : Entity
    {
        public  List<Endpoint> Endpoints { get; set; }
        public ServiceSecret ServiceSecret { get; set; }
    }
}
