using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SEP.Autorization.Interfaces;
using SEP.Common.DTO;
using SEP.Common.Models;
using System.IO;

namespace SEP.Autorization.Controllers
{
    [ApiController]
    [Route("")]
    public class AutorizationController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IAutorizationService autorizationService;
        public AutorizationController(IMapper mapper, IAutorizationService autorizationService)
        {
            this.mapper = mapper;
            this.autorizationService = autorizationService;
        }

        [HttpGet]
        [Route("authKeys")]
        [AllowAnonymous]
        public ActionResult<List<AuthKeyDTO>> GetAuthKeys(string key)
        {
            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            if (key.Equals(appSettings.GetValue<string>("Secrets:AutorizationKey")))
            {
                return mapper.Map<List<AuthKeyDTO>>(autorizationService.GetAuthKeys());
            }  

            return BadRequest(new { message = "key is invalid" });
        }

        [HttpPost]
        [Route("addAuthKey")]
        [AllowAnonymous]
        public ActionResult<List<AuthKeyDTO>> AddAuthKey([FromBody] AuthKeyWithKeyDTO key)
        {
            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            if (key.KeyForAutorization.Equals(appSettings.GetValue<string>("Secrets:AutorizationKey")))
            {
                AuthKey authKey = mapper.Map<AuthKey>(key);
                autorizationService.AddAuthKey(authKey);
                return mapper.Map<List<AuthKeyDTO>>(autorizationService.GetAuthKeys());
            }


            return BadRequest(new { message = "key is invalid" });
        }
    }
}
