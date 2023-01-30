using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nancy.Json;
using SEP.Common.DTO;
using SEP.Common.Enums;
using SEP.PSP.DTO;
using SEP.PSP.Interfaces;
using SEP.PSP.Models;
using System.Net;
using System.Net.Mime;

namespace SEP.PSP.Controllers
{
    [Route("api/psp")]
    [ApiController]
    public class PSPController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PSPController> _logger;
        private readonly IPSPService _PSPService;

        public PSPController(IMapper mapper, ILogger<PSPController> logger, IPSPService PSPService)
        {
            _mapper = mapper;
            _logger = logger;
            _PSPService = PSPService;
        }

        [HttpPost("payPaypal")]
        [Consumes(MediaTypeNames.Application.Json)]
        public string MakePayPalPayment([FromBody] PSPPaymentDTO PSPPaymentDTO)
        {
            _logger.LogInformation("PSP make payment executing...");
            return _PSPService.MakePayPalPayment(PSPPaymentDTO);
        }

        [HttpPost("payBank")]
        [Consumes(MediaTypeNames.Application.Json)]
        public string MakeBankPayment([FromBody] PSPPaymentDTO PSPPaymentDTO)
        {
            _logger.LogInformation("PSP make payment executing...");
            return _PSPService.MakeBankPayment(PSPPaymentDTO);
        }

        [HttpPost("payBitcoin")]
        [Consumes(MediaTypeNames.Application.Json)]
        public void MakeBitcoinPayment([FromBody] PSPBitcoinPaymentDTO PSPBitcoinPaymentDTO)
        {
            _logger.LogInformation("PSP make payment executing...");
            _PSPService.MakeBitcoinPayment(PSPBitcoinPaymentDTO);
            try
            {
                var jss = new JavaScriptSerializer();
                var getdata = string.Empty;
                var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:7035/api/payment/updateBitcoin");
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";

                var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
                httpRequest.Headers["key"] = appSettings.GetValue<string>("Secrets:AutorizationKey");

                var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                streamWriter.Write(jss.Serialize(new StringDTO(PSPBitcoinPaymentDTO.IdentityToken)));
                streamWriter.Close();

                using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
                using (var stream = webresponse.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    getdata = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning("Couldn't contact webshop");
            }
        }

        [HttpPost("payQR")]
        [Consumes(MediaTypeNames.Application.Json)]
        public string MakeQRPayment([FromBody] PSPPaymentDTO PSPPaymentDTO)
        {
            _logger.LogInformation("PSP make payment executing...");
            return _PSPService.MakeQRPayment(PSPPaymentDTO);
        }

        [HttpPost("subscribe")]
        [Consumes(MediaTypeNames.Application.Json)]
        public string SubscribeWebshopToPayment([FromBody] int serviceType)
        {
            _logger.LogInformation("PSP make subscription executing...");
            var subscription = _mapper.Map<Subscription>(new SubscriptionDTO() { Merchant = new(), PaymentMicroserviceType = (PaymentMicroserviceType) serviceType});
            subscription.Merchant.Port = Request.Headers["senderPort"].ToString();
            if (subscription.Merchant.Port.IsNullOrEmpty())
                return string.Empty;

            subscription = _PSPService.SubscribeWebshopToPayment(subscription);
            _logger.LogWarning("PSP make subscription ended...");
            if (subscription != null)
                return subscription.Merchant.Key;
            else
                return "";
        }

        [HttpPost("continue")]
        [Consumes(MediaTypeNames.Application.Json)]
        public void Continue([FromBody] PSPPayPalPaymentDTO pspPayPalPaymentDTO)
        {
            _logger.LogInformation("PSP continue payment executing...");
            _PSPService.EditPayPalPayment(pspPayPalPaymentDTO.ConvertToPSPPayment());
        }

        [HttpPost("update")]
        [Consumes(MediaTypeNames.Application.Json)]
        public void Update([FromBody] PSPBankPaymentDTO pspBankPaymentDTO)
        {
            _logger.LogInformation("PSP update payment executing...");
            _PSPService.UpdatePayment(pspBankPaymentDTO.ConvertToPSPPayment());

            try
            {
                var jss = new JavaScriptSerializer();
                var getdata = string.Empty;
                var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:7035/api/payment/update");
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/json";

                var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
                httpRequest.Headers["key"] = appSettings.GetValue<string>("Secrets:AutorizationKey");

                var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
                streamWriter.Write(jss.Serialize(pspBankPaymentDTO));
                streamWriter.Close();

                using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
                using (var stream = webresponse.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    getdata = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning("Couldn't contact webshop");
            }
        }

        [HttpPost("cancel")]
        [Consumes(MediaTypeNames.Application.Json)]
        public void Cancel([FromBody] PSPPayPalPaymentDTO pspPayPalPaymentDTO)
        {
            _logger.LogInformation("PSP cancel payment executing...");
            _PSPService.EditPayPalPayment(pspPayPalPaymentDTO.ConvertToPSPPayment());
        }

        [HttpGet("subscribed")]
        [Consumes(MediaTypeNames.Application.Json)]
        public IActionResult GetSubscribedByPort()
        {
            return Ok(_PSPService.GetSubscribedByPort(Request.Headers["senderPort"].ToString()));
        }

        [HttpPost("removeServiceType")]
        [Consumes(MediaTypeNames.Application.Json)]
        public Boolean RemoveServiceType([FromBody] int serviceType)
        {
            if(serviceType == 0)
                return _PSPService.RemoveServiceType("Paypal", Request.Headers["senderPort"].ToString());
            else if (serviceType == 1)
                return _PSPService.RemoveServiceType("QR", Request.Headers["senderPort"].ToString());
            else if (serviceType == 2)
                return _PSPService.RemoveServiceType("Card", Request.Headers["senderPort"].ToString());
            else
                return _PSPService.RemoveServiceType("Bitcoin", Request.Headers["senderPort"].ToString());
        }

        [HttpGet("getMerchantByPort")]
        public IActionResult GetMerchantByPort() {
            Merchant merchant = _PSPService.GetMerchantByPort(Request.Headers["senderPort"].ToString());
            return Ok(merchant.Port + "," + merchant.Key);
        }
    }
}
