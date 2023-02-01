using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Net.Mime;
using System.Net;
using SEP.Bank.DTO;
using SEP.Bank.Models;
using SEP.Bank.Interfaces;
using SEP.Common.Enums;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using Newtonsoft.Json;

namespace SEP.Bank.Controllers
{

    [Route("api/bank")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<BankController> _logger;
        private readonly IBankService _bankService;
        private string Pan;
        private string Front;
        private string FrontQR;
        private string Name;
        private string AccountNumber;

        public BankController(IMapper mapper, ILogger<BankController> logger, IBankService bankService)
        {
            _mapper = mapper;
            _logger = logger;
            _bankService = bankService;
            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            Pan = appSettings.GetValue<string>("Pan");
            Front = appSettings.GetValue<string>("FrontUrl");
            FrontQR = appSettings.GetValue<string>("FrontQRUrl");
            Name = appSettings.GetValue<string>("Name");
            AccountNumber = appSettings.GetValue<string>("AccountNumber");

        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public string Redirect([FromBody] BankPaymentWithoutCardDTO bankPaymentDTO)
        {
            _logger.LogInformation("Redirect");
            if (Request.Headers["senderPort"].ToString().Equals("5050"))
            {         
               return "https://localhost:5860/api/bank/get" + "?id=" + _bankService.Save(_mapper.Map<BankPayment>(bankPaymentDTO));
            }

            _logger.LogWarning("You don't have access.");
            return null;
        }

        [HttpPost("qr")]
        [Consumes(MediaTypeNames.Application.Json)]
        public string RedirectQR([FromBody] BankPaymentWithoutCardDTO bankPaymentDTO)
        {
            _logger.LogInformation("Redirect");
            if (Request.Headers["senderPort"].ToString().Equals("5050"))
            {
                return FrontQR+"?id=" + _bankService.Save(_mapper.Map<BankPayment>(bankPaymentDTO));
            }

            _logger.LogWarning("You don't have access.");
            return null;
        }

        [HttpPost("pay")]
        public RedirectResult Pay([FromForm] string id, [FromForm] string securityCode, [FromForm] string number, [FromForm] string month, [FromForm] string year)
        {
            CardDTO cardDTO = new CardDTO(id, month, year, number, securityCode);
            _logger.LogInformation("Check if this bank");
            if (cardDTO.Number.StartsWith(Pan))
            {
                var payment = _mapper.Map<BankPaymentDTO>(_bankService.Pay(cardDTO));
                if (payment != null)
                {
                    var jss = new JavaScriptSerializer();
                    var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/psp/update");
                    httpRequest.Method = "POST";
                    httpRequest.ContentType = "application/json";
                    var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                    streamWriter.Write(jss.Serialize(payment));
                    streamWriter.Close();
                    httpRequest.GetResponse();
                }
                return RedirectPermanent("http://localhost:4200/success");
            } 
            else
            {
                var payment = _mapper.Map<BankPaymentDTO>(_bankService.Convert(cardDTO));
                if (payment != null)
                {
                    _logger.LogInformation("Redirect");
                    var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/pcc");
                    httpRequest.Method = "POST";
                    httpRequest.ContentType = "application/json";
                    var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                    streamWriter.Write(JsonConvert.SerializeObject(payment));
                    streamWriter.Close();
                    
                    var getdata = new BankPaymentDTO();
                    using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
                    using (var stream = webresponse.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        var json = reader.ReadToEnd();
                        getdata = JsonConvert.DeserializeObject<BankPaymentDTO>(json);
                    }

                    var newPayment = _bankService.Update(_mapper.Map<BankPayment>(getdata));
                    var newPaymentDto = _mapper.Map<BankPaymentDTO>(newPayment);

                    var jss = new JavaScriptSerializer();
                    var httpRequest2 = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/psp/update");
                    httpRequest2.Method = "POST";
                    httpRequest2.ContentType = "application/json";
                    var streamWriter2 = new StreamWriter(httpRequest2.GetRequestStream());
                    streamWriter2.Write(jss.Serialize(newPaymentDto));
                    streamWriter2.Close();
                    httpRequest2.GetResponse();
                }
                return RedirectPermanent("http://localhost:4200/success");
            }

            _logger.LogWarning("You don't have access.");

        }

        [HttpGet("get")]
        [Consumes(MediaTypeNames.Application.Json)]
        public ActionResult Get(string id)
        {
            var html = "<!DOCTYPE html>\r\n<html>\r\n<body>\r\n<script>\r\ndocument.addEventListener(\"DOMContentLoaded\", function(){\r\ndocument.getElementById(\"id\").value = " + id + ";\r\n});\r\n</script>\r\n<div>\r\n    <div>\r\n        <h1>Confirm Purchase</h1>\r\n    </div>\r\n    <div>\r\n        <form action=\"" + Front + "\" method=\"post\">\r\n            <input hidden type=\"text\" name=\"id\" id=\"id\">\r\n            <div>\r\n                <label>CVV</label>\r\n                <input type=\"text\" name=\"securityCode\" id=\"securityCode\">\r\n            </div>\r\n            <div id=\"card-number-field\">\r\n                <label>Card Number</label>\r\n                <input type=\"text\" name=\"number\" id=\"number\">\r\n            </div>\r\n            <div id=\"expiration-date\">\r\n                <label>Expiration Date</label>\r\n                <select name=\"month\">\r\n                    <option value=\"1\">January</option>\r\n                    <option value=\"2\">February </option>\r\n                    <option value=\"3\">March</option>\r\n                    <option value=\"4\">April</option>\r\n                    <option value=\"5\">May</option>\r\n                    <option value=\"6\">June</option>\r\n                    <option value=\"7\">July</option>\r\n                    <option value=\"8\">August</option>\r\n                    <option value=\"9\">September</option>\r\n                    <option value=\"10\">October</option>\r\n                    <option value=\"11\">November</option>\r\n                    <option value=\"12\">December</option>\r\n                </select>\r\n                <select name=\"year\">\r\n                    <option value=\"2016\"> 2016</option>\r\n                    <option value=\"2017\"> 2017</option>\r\n                    <option value=\"2018\"> 2018</option>\r\n                    <option value=\"2019\"> 2019</option>\r\n                    <option value=\"2020\"> 2020</option>\r\n                    <option value=\"2021\"> 2021</option>\r\n                    <option value=\"2022\"> 2022</option>\r\n                    <option value=\"2023\"> 2023</option>\r\n\t     <option value=\"2024\"> 2024</option>\r\n                </select>\r\n            </div>\r\n            <div id=\"pay-now\">\r\n                <button type=\"submit\" id=\"confirm-purchase\">Confirm</button>\r\n            </div>\r\n        </form>\r\n    </div>\r\n</div>\r\n</body>\r\n</html>";

            return new ContentResult
            {
                Content = html,
                ContentType = "text/html"
            };
        }

        [HttpGet("get/qr")]
        [Consumes(MediaTypeNames.Application.Json)]
        public ContentResult getQr(string id)
        {
            _logger.LogInformation("RedirectQR");
            string QrUri = null;
            BankPayment bankPayment = _bankService.GetById(id);

            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(bankPayment.Amount + "," + bankPayment.Currency + "," + Name + "," + AccountNumber + "," + FrontQR + "?id=" + id, QRCodeGenerator.ECCLevel.Q);
            QRCode QrCode = new QRCode(QrCodeInfo);
            Bitmap QrBitmap = QrCode.GetGraphic(60);
            byte[] array = null;
            using (MemoryStream ms = new MemoryStream())
            {
                QrBitmap.Save(ms, ImageFormat.Png);
                array = ms.ToArray();
            }
            byte[] BitmapArray = array;
            QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
            

            _logger.LogWarning("You don't have access.");
            var html = "<!DOCTYPE html>\r\n<html>\r\n<body><img src=\"" + QrUri + "\"/><a href=\""+ "https://localhost:5860/api/bank/get" + "?id=" + id+"\">Pay!</a></body>\r\n</html>";
            return new ContentResult
            {
                Content = html,
                ContentType = "text/html"
            };
        }
    }
}