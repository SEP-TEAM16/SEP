using Microsoft.AspNetCore.Mvc;
using SEP.WebShop.Core.Repositories;
using SEP.WebShop.Core.Services;
using SEP.WebShop.Web.DtoFactories;

namespace SEP.WebShop.Web.Controllers
{
    [Route("api/subscriptions")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly SubscriptionService _subscriptionService;
        private readonly SubscriptionDtoFactory _subscriptionDtoFactory;

        public SubscriptionsController(ISubscriptionRepository subscriptionRepository, SubscriptionService subscriptionService)
        {
            _subscriptionRepository = subscriptionRepository;
            _subscriptionService = subscriptionService;
            _subscriptionDtoFactory = new SubscriptionDtoFactory();   
        }

        [HttpGet]
        public IActionResult GetAllOptions()
        {
            return Ok(_subscriptionRepository.FindAll().Select(item => _subscriptionDtoFactory.Create(item)));
        }
    }
}
