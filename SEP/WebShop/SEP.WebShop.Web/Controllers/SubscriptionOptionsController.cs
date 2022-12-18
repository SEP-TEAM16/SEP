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
        private readonly ISubscriptionOptionRepository _subscriptionOptionRepository;
        private readonly SubscriptionOptionService _subscriptionOptionService;
        private readonly SubscriptionOptionDtoFactory _subscriptionOptionDtoFactory;

        public SubscriptionOptionsController(ISubscriptionOptionRepository subscriptionOptionRepository, SubscriptionOptionService subscriptionOptionService)
        {
            _subscriptionOptionRepository = subscriptionOptionRepository;
            _subscriptionOptionService = subscriptionOptionService;
            _subscriptionOptionDtoFactory = new SubscriptionOptionDtoFactory();
        }

        [HttpGet]
        public IActionResult GetAllOptions()
        {
            return Ok(_subscriptionOptionRepository.FindAll().Select(item => _subscriptionOptionDtoFactory.Create(item)));
        }
    }
}
