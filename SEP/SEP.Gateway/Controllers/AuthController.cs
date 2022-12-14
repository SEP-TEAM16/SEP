using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using SEP.Common.DTO;
using SEP.Common.Models;
using SEP.Gateway.Models;
using SEP.Gateway.Services;
using System.Net;

namespace SEP.Gateway.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private List<AuthKey> AuthKeys { get; set; }
        public AuthController()
        {
            AuthKeys = new List<AuthKey>();
            var getdata = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();

            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5250/authKeys");
            httpRequest.Method = "GET";
            httpRequest.ContentType = "application/json";

            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            httpRequest.Headers["key"] = appSettings.GetValue<string>("Secrets:AutorizationKey");

            using (HttpWebResponse webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (Stream stream = webresponse.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                getdata = reader.ReadToEnd();
            }

            AuthKeys = jss.Deserialize<List<AuthKey>>(getdata);
        }

        [HttpPost]
        [Route("{route}")]
        [AllowAnonymous]
        public ActionResult<AuthToken> GetPayPalAuthentication(string route, [FromBody] string key)
        {
            foreach (AuthKey authKey in AuthKeys)
            {
                if (key.Equals(authKey.Key) && route.Equals(authKey.Route))
                    return new PayPalApiTokenService().GenerateToken(authKey.Key);
            }

            return BadRequest(new { message = "key is invalid" });
        }

        [HttpPost]
        [AllowAnonymous]
        public void AddMicroservice([FromBody] AuthKey key)
        {
            var getdata = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();

            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5250/addAuthKey");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";

            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            AuthKeyWithKeyDTO authKeyWithKeyDTO = new AuthKeyWithKeyDTO(key.Key, key.Route, appSettings.GetValue<string>("Secrets:AutorizationKey"));
            streamWriter.Write(jss.Serialize(authKeyWithKeyDTO));
            streamWriter.Close();

            using (HttpWebResponse webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (Stream stream = webresponse.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                getdata = reader.ReadToEnd();
            }

            AuthKeys = jss.Deserialize<List<AuthKey>>(getdata);
        }
    }
}
