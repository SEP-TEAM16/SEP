using System.ComponentModel.DataAnnotations;

namespace SEP.Common.Models
{
    public class AuthKey : Entity
    {
        public string Key { get; set; }
        public string Route { get; set; }

        public AuthKey(string key, string route, int id = 0)
        {
            Key = key;
            Route = route;
        }
    }
}
