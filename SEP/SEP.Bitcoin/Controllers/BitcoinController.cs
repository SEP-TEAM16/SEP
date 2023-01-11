using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SEP.Bitcoin.DTO;
using SEP.Bitcoin.Interfaces;
using SEP.Bitcoin.Models;
using System.Net.Mime;

namespace SEP.Bitcoin.Controllers
{
    [Route("api/bitcoin")]
    [ApiController]
    public class BitcoinController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<BitcoinController> _logger;
        private readonly IBitcoinService _bitcoinService;

        public BitcoinController(IMapper mapper, ILogger<BitcoinController> logger, IBitcoinService bitcoinService)
        {
            _mapper = mapper;
            _logger = logger;
            _bitcoinService = bitcoinService;
        }


        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public BitcoinPaymentDTO Pay([FromBody] BitcoinPaymentDTO bitcoinPaymentDTO)
        {
            _logger.LogInformation("Pay bitcoin");
            if (Request.Headers["senderPort"].ToString().Equals("5050"))
            {
                var bitcoinPayment = _mapper.Map<BitcoinPayment>(bitcoinPaymentDTO);
                return _mapper.Map<BitcoinPaymentDTO>(_bitcoinService.Pay(bitcoinPayment));
            }

            _logger.LogWarning("You don't have access.");
            return null;
        }

    }
}