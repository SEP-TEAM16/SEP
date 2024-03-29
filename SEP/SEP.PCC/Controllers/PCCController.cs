using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Net.Mime;
using System.Net;
using SEP.PCC.DTO;
using Newtonsoft.Json;
using Nancy;

namespace SEP.PCC.Controllers
{

    [Route("api/pcc")]
    [ApiController]
    public class PCCController : ControllerBase
    {
        private readonly ILogger<PCCController> _logger;

        public PCCController(ILogger<PCCController> logger)
        { 
            _logger = logger;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public BankPaymentDTO Redirect([FromBody] BankPaymentDTO bankPaymentDTO)
        {
            _logger.LogInformation("Redirect");
            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/bank2/pay");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(JsonConvert.SerializeObject(bankPaymentDTO));
            streamWriter.Close();
            //httpRequest.GetResponse();

            var getdata = new BankPaymentDTO();
            using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (var stream = webresponse.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                getdata = JsonConvert.DeserializeObject<BankPaymentDTO>(json);
            }

            return getdata;
        }


    }
}