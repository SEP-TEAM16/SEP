﻿using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nancy.Json;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Repositories;
using SEP.WebShop.Core.Services;
using SEP.WebShop.Web.Dto;
using SEP.WebShop.Web.DtoFactories;
using System.Net;
using System.Net.Mime;

namespace SEP.WebShop.Web.Controllers
{
    [Route("api/subscriptions")]
    [Authorize]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ILogger<SubscriptionsController> _logger;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly SubscriptionService _subscriptionService;
        private readonly SubscriptionDtoFactory _subscriptionDtoFactory;

        public SubscriptionsController(ILogger<SubscriptionsController> logger, ISubscriptionRepository subscriptionRepository, SubscriptionService subscriptionService)
        {
            _logger = logger;
            _subscriptionRepository = subscriptionRepository;
            _subscriptionService = subscriptionService;
            _subscriptionDtoFactory = new SubscriptionDtoFactory();
        }

        [HttpGet]
        public IActionResult GetAllSubscriptions()
        {
            _logger.LogInformation("WebShop subscriptions GetAll executing...");
            return Ok(_subscriptionRepository.FindAll().Select(item => _subscriptionDtoFactory.Create(item)));
        }

        /*[AllowAnonymous]
        [HttpPost]
        public IActionResult MakeSubscription()
        {
            _logger.LogInformation("WebShop subscriptions MakeSubscription executing...");
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
        }*/

        [HttpPost]
        [AllowAnonymous]
        public Boolean SubscribeWebshop([FromBody] int serviceType)
        {
            var jss = new JavaScriptSerializer();

            var httpRequest = (HttpWebRequest)HttpWebRequest.Create("https://localhost:7038/api/psp/subscribe");
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers.Add("senderPort", Request.Headers["senderPort"].ToString());

            var streamWriter = new StreamWriter(httpRequest.GetRequestStream());
            streamWriter.Write(jss.Serialize(serviceType));
            streamWriter.Close();

            string getdata = string.Empty;
            using (var webresponse = (HttpWebResponse)httpRequest.GetResponse())
            using (var stream = webresponse.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                getdata = reader.ReadToEnd();
            }

            if (getdata == "")
                return false;
            else
                return true;
        }
    }
}
