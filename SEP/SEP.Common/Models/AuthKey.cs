using System.ComponentModel.DataAnnotations;

namespace SEP.Common.Models
{
    public class AuthKey
    {
        public string Key { get; set; }
        public string Route { get; set; }

        public AuthKey(string key, string route)
        {
            Key = key;
            Route = route;
        }
    }
}
