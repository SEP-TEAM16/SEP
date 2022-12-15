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
        private readonly Interfaces.IAuthorizationService autorizationService;
        public AutorizationController(IMapper mapper, Interfaces.IAuthorizationService autorizationService)
        {
            this.mapper = mapper;
            this.autorizationService = autorizationService;
        }

        [HttpGet]
        [Route("authKeys")]
        [AllowAnonymous]
        public ActionResult<List<AuthKeyDTO>> GetAuthKeys()
        {
            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            if (Request.Headers["key"].Equals(appSettings.GetValue<string>("Secrets:AutorizationKey")))
            {
                return mapper.Map<List<AuthKeyDTO>>(autorizationService.GetAuthKeys());
            }  

            return BadRequest(new { message = "key is invalid" });
        }

        [HttpPost]
        [Route("addAuthKey")]
        [AllowAnonymous]
        public ActionResult<List<AuthKeyDTO>> AddAuthKey([FromBody] List<AuthKeyWithKeyDTO> keys)
        {
            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            foreach (var key in keys)
            {
                if (key.KeyForAutorization.Equals(appSettings.GetValue<string>("Secrets:AutorizationKey")))
                {
                    AuthKey authKey = mapper.Map<AuthKey>(key);
                    if (!autorizationService.AddAuthKey(authKey))
                        return BadRequest(new { message = "route already exists" });
                } else
                {
                    return BadRequest(new { message = "key is invalid" });
                }
            }

            return mapper.Map<List<AuthKeyDTO>>(autorizationService.GetAuthKeys());

        }
    }
}
