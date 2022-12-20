﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using SEP.Common.Enums;
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
        public void Continue(string paymentId, string token, string payerId)
        {
            _logger.LogInformation("Pay pal continue executing...");
            var payPalPayment = _payPalService.Pay(paymentId, payerId, token);
            var payPalPaymentDTO = _mapper.Map<PayPalPaymentDTO>(payPalPayment);
            _logger.LogInformation("Sending continue to psp...");
            var jss = new JavaScriptSerializer();
            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/psp/continue");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(jss.Serialize(payPalPaymentDTO));
            streamWriter.Close();
            httpRequest.GetResponse();
            return;
        }

        [HttpGet("cancel")]
        public void Cancel(string token)
        {
            _logger.LogInformation("Pay pal cancel executing...");
            var payPalPaymentDTO = _mapper.Map<PayPalPaymentDTO>(_payPalService.Cancel(token));
            _logger.LogInformation("Sending cancel to psp...");
            var jss = new JavaScriptSerializer();
            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:5050/psp/cancel");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(jss.Serialize(payPalPaymentDTO));
            streamWriter.Close();
            httpRequest.GetResponse();
            return;
        }
    }
}