using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using System.Net.Mime;
using System.Net;
using SEP.Bank.DTO;
using SEP.Bank.Models;
using SEP.Bank.Interfaces;
using SEP.Common.Enums;

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

        public BankController(IMapper mapper, ILogger<BankController> logger, IBankService bankService)
        {
            _mapper = mapper;
            _logger = logger;
            _bankService = bankService;
            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            Pan = appSettings.GetValue<string>("PaymentMicroserviceType");
            Front = appSettings.GetValue<string>("FrontUrl");
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public string Redirect([FromBody] BankPaymentWithoutCardDTO bankPaymentDTO)
        {
            _logger.LogInformation("Redirect");
            if (Request.Headers["senderPort"].ToString().Equals("5050"))
            {         
               return Front + "?id=" + _bankService.Save(_mapper.Map<BankPayment>(bankPaymentDTO));
            }

            _logger.LogWarning("You don't have access.");
            return null;
        }

        [HttpPost("pay")]
        [Consumes(MediaTypeNames.Application.Json)]
        public BankPaymentDTO Pay([FromBody] BankPaymentDTO bankPaymentDTO)
        {
            _logger.LogInformation("Check if this bank");
            if (bankPaymentDTO.Number.StartsWith(Pan))
            {
                var bankPayment = _mapper.Map<BankPayment>(bankPaymentDTO);
                return _mapper.Map<BankPaymentDTO>(_bankService.Pay(bankPayment));
            }

            _logger.LogWarning("You don't have access.");
            return null;
        }
    }
}