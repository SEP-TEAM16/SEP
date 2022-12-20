using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SEP.WebShop.Core.Repositories;

namespace SEP.WebShop.Web.Controllers
{
    [Route("api/payment")]
    [Authorize]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentRepository _paymentRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public PaymentController(ILogger<PaymentController> logger, IPaymentRepository paymentRepository, ISubscriptionRepository subscriptionRepository)
        {
            _logger = logger;
            _paymentRepository = paymentRepository;
            _subscriptionRepository = subscriptionRepository;
        }

        [HttpPost]
        public IActionResult MakeAPayment()
        {
            _logger.LogInformation("WebShop make payment executing...");
            return Ok();
        }
    }
}
