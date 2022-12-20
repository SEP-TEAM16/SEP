using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Entities.Enums;
using SEP.WebShop.Core.Entities.ValueObjects;
using SEP.WebShop.Core.Repositories;
using SEP.WebShop.Core.Services;
using SEP.WebShop.Web.Authorization;
using SEP.WebShop.Web.Dto;
using SEP.WebShop.Web.RabbitMQ;
using System.Net;

namespace SEP.WebShop.Web.Controllers
{
    [Route("api/payment")]
    [Authorize]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly PaymentService _paymentService;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IWebShopUserRepository _userRepository;
        private readonly IJwtUtils _jwtUtils;
        private IMessageProducer _messageProducer;

        public PaymentController(ILogger<PaymentController> logger, PaymentService paymentService, ISubscriptionRepository subscriptionRepository, IWebShopUserRepository userRepository, IJwtUtils jwtUtils, IMessageProducer messageProducer)
        {
            _logger = logger;
            _paymentService = paymentService;
            _subscriptionRepository = subscriptionRepository;
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
            _messageProducer = messageProducer;
        }

        [HttpPost]
        public IActionResult MakeAPayment()
        {
            _logger.LogInformation("WebShop make payment executing...");
            var paymentDto = new PaymentDto();
            if (!_jwtUtils.ValidateToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()).HasValue)
                return null;
            var user = _userRepository.FindById(_jwtUtils.ValidateToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()).Value).Value;
            paymentDto.Merchant = new MerchantDto(0, "dasd", "7035");
            paymentDto.Currency = "USD";
            paymentDto.Email = user.EmailAddress;
            if (user.UserType.Equals(UserType.candidate))
            {
                paymentDto.FirstName = user.Name.ToString().Split(" ")[0];
                paymentDto.LastName = user.Name.ToString().Split(" ")[1];
            } 
            else
            {
                paymentDto.FirstName = user.Name.ToString();
                paymentDto.LastName = user.Name.ToString();
                paymentDto.Name = user.Name.ToString();
            }
            paymentDto.IdentityToken = Guid.NewGuid().ToString();
            paymentDto.ItemName = "Package 1";
            paymentDto.Amount = 100;
            paymentDto.Description = "description";
            paymentDto.Key = "sadasdnasd";
            var payment = _paymentService.Create(Payment.Create(Guid.NewGuid(), paymentDto.ItemName, paymentDto.Amount, paymentDto.Currency, Guid.NewGuid(), PaymentStatus.pending).Value);
            //_messageProducer.SendMessage<PaymentDto>(paymentDto, "makePayment", "7035");

            var jss = new JavaScriptSerializer();

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:7038/api/psp/payPaypal");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";

            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(jss.Serialize(paymentDto));
            streamWriter.Close();

            var getdata = string.Empty;
            using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (var stream = webresponse.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                getdata = reader.ReadToEnd();
            }

            return Ok(getdata);
        }
    }
}
