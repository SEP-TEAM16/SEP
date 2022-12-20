using Microsoft.AspNetCore.Mvc;
using SEP.WebShop.Core.Repositories;
using SEP.WebShop.Core.Services;
using SEP.WebShop.Web.DtoFactories;

namespace SEP.WebShop.Web.Controllers
{
    [Route("api/subscription-options")]
    [ApiController]
    public class SubscriptionOptionsController : ControllerBase
    {
        private readonly ILogger<SubscriptionOptionsController> _logger;
        private readonly ISubscriptionOptionRepository _subscriptionOptionRepository;
        private readonly SubscriptionOptionService _subscriptionOptionService;
        private readonly SubscriptionOptionDtoFactory _subscriptionOptionDtoFactory;

        public SubscriptionOptionsController(ILogger<SubscriptionOptionsController> logger, ISubscriptionOptionRepository subscriptionOptionRepository, SubscriptionOptionService subscriptionOptionService)
        {
            _logger = logger;
            _subscriptionOptionRepository = subscriptionOptionRepository;
            _subscriptionOptionService = subscriptionOptionService;
            _subscriptionOptionDtoFactory = new SubscriptionOptionDtoFactory();
        }

        [HttpGet]
        public IActionResult GetAllOptions()
        {
            _logger.LogInformation("WebShop subscription options GetAll executing...");
            return Ok(_subscriptionOptionRepository.FindAll().Select(item => _subscriptionOptionDtoFactory.Create(item)));
        }
    }
}
