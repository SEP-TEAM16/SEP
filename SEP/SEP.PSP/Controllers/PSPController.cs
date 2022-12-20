using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SEP.Common.Enums;
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

        [HttpPost("payPaypal")]
        [Consumes(MediaTypeNames.Application.Json)]
        public string MakePayPalPayment([FromBody] PSPPaymentDTO PSPPaymentDTO)
        {
            _logger.LogInformation("PSP make payment executing...");
            return _PSPService.MakePayPalPayment(PSPPaymentDTO);
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
            return subscription.Merchant.Key;
        }

        [HttpPost("continue")]
        [Consumes(MediaTypeNames.Application.Json)]
        public void Continue([FromBody] PSPPayPalPaymentDTO pspPayPalPaymentDTO)
        {
            _logger.LogInformation("PSP continue payment executing...");
            _PSPService.EditPayPalPayment(pspPayPalPaymentDTO.ConvertToPSPPayment());
        }

        [HttpPost("cancel")]
        [Consumes(MediaTypeNames.Application.Json)]
        public void Cancel([FromBody] PSPPayPalPaymentDTO pspPayPalPaymentDTO)
        {
            _logger.LogInformation("PSP cancel payment executing...");
            _PSPService.EditPayPalPayment(pspPayPalPaymentDTO.ConvertToPSPPayment());
        }
    }
}
