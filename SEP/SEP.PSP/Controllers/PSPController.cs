using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SEP.PSP.DTO;
using SEP.PSP.Interfaces;
using SEP.PSP.Models;
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

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public PSPPaymentDTO MakePayPalPayment([FromBody] PSPPaymentDTO PSPPaymentDTO)
        {
            _logger.LogInformation("PSP make payment executing...");
            return _PSPService.MakePayPalPayment(PSPPaymentDTO);

        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public string SubscribeWebshopToPayment([FromBody] SubscriptionDTO subscriptionDTO)
        {
            _logger.LogInformation("PSP make subscription executing...");
            var subscription = _mapper.Map<Subscription>(subscriptionDTO);
            subscription.Merchant.Port = Request.Headers["senderPort"].ToString();
            if (subscription.Merchant.Port.IsNullOrEmpty())
            {
                return "";
            }
            subscription = _PSPService.SubscribeWebshopToPayment(subscription);
            _logger.LogWarning("PSP make subscription ended...");
            return subscription.Merchant.Key;
        }
    }
}
