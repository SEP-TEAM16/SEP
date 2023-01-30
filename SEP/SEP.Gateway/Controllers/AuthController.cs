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
        private readonly ILogger<AuthController> _logger;
        private readonly object _object = new object();
        private List<AuthKey> AuthKeys { get; set; }

        public AuthController(ILogger<AuthController> logger)
        {
            lock (_object)
            {
                _logger = logger;

                AuthKeys = new List<AuthKey>();
                var getdata = string.Empty;
                var jss = new JavaScriptSerializer();

                var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5250/authKeys");
                httpRequest.Method = "GET";
                httpRequest.ContentType = "application/json";

                var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
                httpRequest.Headers["key"] = appSettings.GetValue<string>("Secrets:AutorizationKey");

                using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
                using (var stream = webresponse.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    getdata = reader.ReadToEnd();
                }

                AuthKeys = jss.Deserialize<List<AuthKey>>(getdata);
                if (AuthKeys.Count == 0)
                {
                    var item = new MicroservicesDTO
                    {
                        GlobalConfiguration = new GlobalConfigurationDTO("https://localhost:5050"),
                        Routes = new()
                    };

                    var sw = new StreamWriter("microservices.json");
                    sw.Write(jss.Serialize(item));
                    sw.Close();

                }
            }
        }

        [HttpPost]
        [Route("{route}")]
        [AllowAnonymous]
        public ActionResult<AuthToken> GetAuthentication1(string route, [FromBody] StringDTO key)
        {
            _logger.LogInformation("Gateway get authentication executing...");
            foreach (var authKey in AuthKeys)
            {
                if (key.Value.Equals(authKey.Key) && route.Equals(authKey.Route))
                    return PayPalApiTokenService.GenerateToken(authKey.Key);
            }

            _logger.LogWarning("Key is invalid.");
            return BadRequest(new { message = "key is invalid" });
        }

        [HttpPost]
        [Route("{route}/{route2}")]
        [AllowAnonymous]
        public ActionResult<AuthToken> GetAuthentication(string route, [FromBody] StringDTO key)
        {
            _logger.LogInformation("Gateway get authentication executing...");
            foreach (var authKey in AuthKeys)
            {
                if (key.Value.Equals(authKey.Key) && route.Equals(authKey.Route))
                    return PayPalApiTokenService.GenerateToken(authKey.Key);
            }

            _logger.LogWarning("Key is invalid.");
            return BadRequest(new { message = "key is invalid" });
        }

        [HttpPost]
        [AllowAnonymous]
        public void AddMicroservice([FromBody] List<AuthKeyWithPortDTO> keys)
        {
            lock (_object)
            {
                _logger.LogInformation("Gateway add microservice executing...");

                var getdata = string.Empty;
                var jss = new JavaScriptSerializer();

                var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5250/addAuthKey");
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";

                var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
                var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                var authKeysWithKeyDTOs = new List<AuthKeyWithKeyDTO>();
                foreach (var key in keys)
                {
                    authKeysWithKeyDTOs.Add(new AuthKeyWithKeyDTO(key.Key, key.Route, appSettings.GetValue<string>("Secrets:AutorizationKey"), key.Type, (int)key.PaymentMicroserviceType));
                }
                streamWriter.Write(jss.Serialize(authKeysWithKeyDTOs));
                streamWriter.Close();

                using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
                    if (webresponse.StatusCode == HttpStatusCode.OK)
                    {
                        using (var stream = webresponse.GetResponseStream())
                        using (var reader = new StreamReader(stream))
                        {
                            getdata = reader.ReadToEnd();
                        }

                        AuthKeys = jss.Deserialize<List<AuthKey>>(getdata);
                        var item = new MicroservicesDTO();

                        using (var r = new StreamReader("microservices.json"))
                        {
                            var json = r.ReadToEnd();
                            r.Close();
                            item = JsonConvert.DeserializeObject<MicroservicesDTO>(json);
                        }

                        item ??= new MicroservicesDTO();
                        item.Routes ??= new List<RouteDTO>();

                        foreach (var key in keys)
                        {
                            var httpMethods = new List<string>
                    {
                        key.Type
                    };
                            var authOptinons = new AuthentificationOptionsDTO("auth_scheme", new List<string>());
                            var downstreamPort = new DownstreamHostAndPortsDTO("localhost", key.Port);
                            var downstreamPorts = new List<DownstreamHostAndPortsDTO>
                    {
                        downstreamPort
                    };
                            if (key.NeedAuth)
                            {
                                var senderPort = "5050";
                                var upstreamHeaderTransform = new UpstreamHeaderTransformDTO(senderPort);
                                item.Routes.Add(new RouteDTO("/" + key.Route, httpMethods, "/api/" + key.Route, "https", authOptinons, downstreamPorts, upstreamHeaderTransform));
                            }
                            else
                            {
                                item.Routes.Add(new RouteDTO("/" + key.Route, httpMethods, "/api/" + key.Route, "https", downstreamPorts));
                            }
                        }

                        var sw = new StreamWriter("microservices.json");
                        sw.Write(jss.Serialize(item));
                        sw.Close();

                    }
            }
        }
    }
}
