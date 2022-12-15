using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using SEP.Common.DTO;
using SEP.Common.Models;
using SEP.Gateway.DTO;
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
            if(AuthKeys.Count == 0)
            {
                var item = new MicroservicesDTO();
                item.GlobalConfiguration = new GlobalConfigurationDTO("https://localhost:5050");
                item.Routes = new List<RouteDTO>();
                StreamWriter sw = new StreamWriter("microservices.json");
                sw.Write(jss.Serialize(item));
                sw.Close();
            }
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
        public void AddMicroservice([FromBody] List<AuthKeyWithPortDTO> keys)
        {
            var getdata = "";
            JavaScriptSerializer jss = new JavaScriptSerializer();

            HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5250/addAuthKey");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";

            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            List<AuthKeyWithKeyDTO> authKeysWithKeyDTOs = new List<AuthKeyWithKeyDTO>();
            foreach (var key in keys)
            {
                authKeysWithKeyDTOs.Add(new AuthKeyWithKeyDTO(key.Key, key.Route, appSettings.GetValue<string>("Secrets:AutorizationKey"), key.Type));
            }
            streamWriter.Write(jss.Serialize(authKeysWithKeyDTOs));
            streamWriter.Close();

            using (HttpWebResponse webresponse = (HttpWebResponse)httpRequest.GetResponse())
            if(webresponse.StatusCode == HttpStatusCode.OK)
            {
                    using (Stream stream = webresponse.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        getdata = reader.ReadToEnd();
                    }

                    AuthKeys = jss.Deserialize<List<AuthKey>>(getdata);
                    MicroservicesDTO item = new MicroservicesDTO();
                    using (StreamReader r = new StreamReader("microservices.json"))
                    {
                        string json = r.ReadToEnd();
                        item = JsonConvert.DeserializeObject<MicroservicesDTO>(json);

                    }
                    if (item.Routes == null)
                    {
                        item.Routes = new List<RouteDTO>();
                    }
                    foreach (var key in keys)
                    {
                        var httpMethods = new List<string>();
                        httpMethods.Add(key.Type);
                        var authOptinons = new AuthentificationOptionsDTO("auth_scheme", new List<string>());
                        var downstreamPort = new DownstreamHostAndPortsDTO("localhost", key.Port);
                        var downstreamPorts = new List<DownstreamHostAndPortsDTO>();
                        downstreamPorts.Add(downstreamPort);
                        if (key.NeedAuth)
                        {
                            string senderPort = "5050";
                            UpstreamHeaderTransformDTO upstreamHeaderTransform = new UpstreamHeaderTransformDTO(senderPort);
                            item.Routes.Add(new RouteDTO("/" + key.Route, httpMethods, "/api/" + key.Route, "https", authOptinons, downstreamPorts, upstreamHeaderTransform));
                        } else
                        {
                            item.Routes.Add(new RouteDTO("/" + key.Route, httpMethods, "/api/" + key.Route, "https", downstreamPorts));
                        }
                        
                    }
                    StreamWriter sw = new StreamWriter("microservices.json");
                    sw.Write(jss.Serialize(item));
                    sw.Close();
                } 
        }
    }
}
