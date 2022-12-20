﻿using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Repositories;
using SEP.WebShop.Core.Services;
using SEP.WebShop.Web.Dto;
using SEP.WebShop.Web.DtoFactories;

namespace SEP.WebShop.Web.Controllers
{
    [Route("api/subscriptions")]
    [Authorize]
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
        public IActionResult GetAllSubscriptions()
        {
            return Ok(_subscriptionRepository.FindAll().Select(item => _subscriptionDtoFactory.Create(item)));
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult MakeSubscription()
        {

            //Maybe<WebShopUser> user = _subscriptionService.FindByUsername(authenticationRequest.Username);

            //if (user.HasNoValue)
            //    return BadRequest(new { message = "User with specified username does not exists" });

            //bool isPasswordVerified = user.Value.Password.Verify(authenticationRequest.Password);

            //if (!isPasswordVerified)
            //{
            //    return BadRequest(new { message = "Password you entered does not match user's password" });
            //}

            //var response = new AuthenticationResponse(user.Value, _jwtUtils.GenerateToken(user.Value));

            return Ok();
        }
    }
}
