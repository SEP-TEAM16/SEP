using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SEP.PayPal.DTO;
using SEP.PayPal.Interfaces;
using SEP.PayPal.Models;
using System.Net.Mime;

namespace SEP.PayPal.Controllers
{
    [Route("api/paypal")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<PayPalController> _logger;
        private readonly IPayPalService _payPalService;

        public PayPalController(IMapper mapper, ILogger<PayPalController> logger, IPayPalService payPalService)
        {
            _mapper = mapper;
            _logger = logger;
            _payPalService = payPalService;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public string GetApprovalLink([FromBody] PayPalPaymentDTO payPalPaymentDTO)
        {
            _logger.LogInformation("Pay pal get approval link executing...");
            if (Request.Headers["senderPort"].ToString().Equals("5050"))
            {
                var payPalPayment = _mapper.Map<PayPalPayment>(payPalPaymentDTO);
                return _payPalService.GetApprovalLink(payPalPayment);
            }

            _logger.LogWarning("You don't have access.");
            return "You don't have access";
        }

        [HttpGet("continue")]
        public ActionResult<PayPalPaymentDTO> Continue(string paymentId, string token, string payerId)
        {
            _logger.LogInformation("Pay pal continue executing...");
            var payPalPayment = _payPalService.Pay(paymentId, payerId, token);
            var payPalPaymentDTO = _mapper.Map<PayPalPaymentDTO>(payPalPayment);
            return payPalPaymentDTO;
        }

        [HttpGet("cancel")]
        public void Cancel(string token)
        {
            _logger.LogInformation("Pay pal cancel executing...");
            _payPalService.Cancel(token);
        }
    }
}
