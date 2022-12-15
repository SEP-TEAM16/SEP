using System.ComponentModel.DataAnnotations;

namespace SEP.Common.Models
{
    public class AuthKey : Entity
    {
        public string Key { get; set; }
        public string Route { get; set; }
        public string Type { get; set; }

        public AuthKey(string key, string route, string type, int id)
        {
            Id = id;
            Key = key;
            Route = route;
            Type = type;
        }
    }
}
