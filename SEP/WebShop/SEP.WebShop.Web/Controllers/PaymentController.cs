using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Entities.Enums;
using SEP.WebShop.Core.Entities.ValueObjects;
using SEP.WebShop.Core.Repositories;
using SEP.WebShop.Core.Services;
using SEP.WebShop.Web.Authorization;
using SEP.WebShop.Web.Dto;
using SEP.WebShop.Web.RabbitMQ;
using System.Net;
using System.Net.Mime;
using System.Text.Json.Nodes;

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
        private readonly IPaymentRepository _paymentRepository;
        private readonly IWebShopUserRepository _userRepository;
        private readonly IJwtUtils _jwtUtils;
        private IMessageProducer _messageProducer;

        public PaymentController(ILogger<PaymentController> logger, PaymentService paymentService, IPaymentRepository paymentRepository, ISubscriptionRepository subscriptionRepository, IWebShopUserRepository userRepository, IJwtUtils jwtUtils, IMessageProducer messageProducer)
        {
            _logger = logger;
            _paymentService = paymentService;
            _paymentRepository = paymentRepository; 
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
            //var payment = _paymentService.Create(Payment.Create(Guid.NewGuid(), paymentDto.ItemName, paymentDto.Amount, paymentDto.Currency, Guid.NewGuid(), PaymentStatus.pending, paymentDto.IdentityToken).Value);
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

        [HttpPost("bitcoin")]
        public IActionResult MakeABitcoinPayment()
        {
            _logger.LogInformation("WebShop make payment executing...");
            var paymentDto = new PaymentBitcoinDto();
            if (!_jwtUtils.ValidateToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()).HasValue)
                return null;
            var user = _userRepository.FindById(_jwtUtils.ValidateToken(Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last()).Value).Value;
            paymentDto.Merchant = new MerchantDto(0, "dasd", "7035");
            paymentDto.Currency = "USD";
            paymentDto.PublicKey = "jdaskjdjasdjaskdaskjdjkas";
            paymentDto.PrivateKey = "jdaskjdjasdjaskdaskjdjkas";
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
            //var payment = _paymentService.Create(Payment.Create(Guid.NewGuid(), paymentDto.ItemName, paymentDto.Amount, paymentDto.Currency, Guid.NewGuid(), PaymentStatus.pending, paymentDto.IdentityToken).Value);
            //_messageProducer.SendMessage<PaymentDto>(paymentDto, "makePayment", "7035");

            var jss = new JavaScriptSerializer();

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:7038/api/psp/payBitcoin");
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
            //potrebno sacuvati vracen odgovor
            return Ok(getdata);
        }

        [HttpPost("bank")]
        public IActionResult MakeABankPayment()
        {
            _logger.LogInformation("WebShop make payment executing...");
            var paymentDto = new PaymentBankDto();
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
            //var payment = _paymentService.Create(Payment.Create(Guid.NewGuid(), paymentDto.ItemName, paymentDto.Amount, paymentDto.Currency, Guid.NewGuid(), PaymentStatus.pending, paymentDto.IdentityToken).Value);
            //_messageProducer.SendMessage<PaymentDto>(paymentDto, "makePayment", "7035");

            var jss = new JavaScriptSerializer();

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:7038/api/psp/payBank");
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

        [HttpPost("qr")]
        public IActionResult MakeAQRPayment()
        {
            _logger.LogInformation("WebShop make payment executing...");
            var paymentDto = new PaymentBankDto();
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
            //var payment = _paymentService.Create(Payment.Create(Guid.NewGuid(), paymentDto.ItemName, paymentDto.Amount, paymentDto.Currency, Guid.NewGuid(), PaymentStatus.pending, paymentDto.IdentityToken).Value);
            //_messageProducer.SendMessage<PaymentDto>(paymentDto, "makePayment", "7035");

            var jss = new JavaScriptSerializer();

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:7038/api/psp/payQR");
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

        [HttpPost("continue")]
        [AllowAnonymous]
        public IActionResult ContinuePayment(string identityToken)
        {
            _logger.LogInformation("WebShop payment continue reached. Payment successful...");
            Maybe<Payment> paymentResult = _paymentRepository.FindByIdentityToken(identityToken);
            if (paymentResult.HasNoValue)
            {
                return BadRequest("Payment with specified identity token doesn't exist");
            }
            var payment = paymentResult.Value;
            if (_paymentService.Update(Payment.Create(payment.Id, payment.ItemName, payment.Price, payment.Currency, payment.SubscriptionId, PaymentStatus.success, payment.IdentityToken).Value).IsFailure)
            {
                return BadRequest("Couldn't update payment");
            }
            return Ok();
        }

        [HttpPost("updateBitcoin")]
        [AllowAnonymous]
        public IActionResult UpdateBitcoin(string identityToken)
        {
            _logger.LogInformation("WebShop payment continue reached. Payment successful...");
            Maybe<Payment> paymentResult = _paymentRepository.FindByIdentityToken(identityToken);
            if (paymentResult.HasNoValue)
            {
                return BadRequest("Payment with specified identity token doesn't exist");
            }
            var payment = paymentResult.Value;
            if (_paymentService.Update(Payment.Create(payment.Id, payment.ItemName, payment.Price, payment.Currency, payment.SubscriptionId, PaymentStatus.success, payment.IdentityToken).Value).IsFailure)
            {
                return BadRequest("Couldn't update payment");
            }
            return Ok();
        }

        [HttpPost("update")]
        [AllowAnonymous]
        public IActionResult UpdatePayment([FromBody] PaymentBankDto paymentBankDto)
        {
            _logger.LogInformation("WebShop payment update reached. Payment successful...");
            Maybe<Payment> paymentResult = _paymentRepository.FindByIdentityToken(paymentBankDto.IdentityToken);
            if (paymentResult.HasNoValue)
            {
                return BadRequest("Payment with specified identity token doesn't exist");
            }
            var payment = paymentResult.Value;
            if (_paymentService.Update(Payment.Create(payment.Id, payment.ItemName, payment.Price, payment.Currency, payment.SubscriptionId, PaymentStatus.success, payment.IdentityToken).Value).IsFailure)
            {
                return BadRequest("Couldn't update payment");
            }
            return Ok();
        }

        [HttpPost("cancel")]
        [AllowAnonymous]
        public IActionResult CancelPayment(string identityToken)
        {
            _logger.LogInformation("WebShop payment cancel reached. Payment canceled...");
            Maybe<Payment> paymentResult = _paymentRepository.FindByIdentityToken(identityToken);
            if (paymentResult.HasNoValue)
            {
                return BadRequest("Payment with specified identity token doesn't exist");
            }
            var payment = paymentResult.Value;
            if(_paymentService.Update(Payment.Create(payment.Id, payment.ItemName, payment.Price, payment.Currency, payment.SubscriptionId, PaymentStatus.canceled, payment.IdentityToken).Value).IsFailure)
            {
                return BadRequest("Couldn't update payment");
            }
            return Ok();
        }
    }
}
