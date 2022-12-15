using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using SEP.Common.Models;
using SEP.PayPal.DTO;
using SEP.PayPal.Interfaces;
using SEP.PayPal.Models;
using System.Net;
using System.Net.Mime;

namespace SEP.PayPal.Controllers
{
    [Route("api/paypal")]
    [ApiController]
    public class PayPalController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IPayPalService payPalService;
        public PayPalController(IMapper mapper, IPayPalService payPalService)
        {
            this.mapper = mapper;
            this.payPalService = payPalService;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public string GetApprovalLink([FromBody] PayPalPaymentDTO payPalPaymentDTO)
        {
            HttpRequest request = Request;
            if (request.Headers["senderPort"].ToString().Equals("5050"))
            {
                PayPalPayment payPalPayment = mapper.Map<PayPalPayment>(payPalPaymentDTO);
                return payPalService.GetApprovalLink(payPalPayment);
            }
            return "You don't have access";
        }

        [HttpGet("continue")]
        public bool Continue(string paymentId, string token, string payerId)
        {
            return payPalService.Pay(paymentId, payerId, token);
        }

        [HttpGet("cancel")]
        public void Cancel(string token)
        {
            payPalService.Cancel(token);
        }
    }
}
