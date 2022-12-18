using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SEP.Common.DTO;
using SEP.Common.Models;

namespace SEP.Autorization.Controllers
{
    [ApiController]
    [Route("")]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorizationController> _logger;
        private readonly Interfaces.IAuthorizationService _autorizationService;
        public AuthorizationController(IMapper mapper, ILogger<AuthorizationController> logger, Interfaces.IAuthorizationService autorizationService)
        {
            _mapper = mapper;
            _logger = logger;
            _autorizationService = autorizationService;
        }

        [HttpGet]
        [Route("authKeys")]
        [AllowAnonymous]
        public ActionResult<List<AuthKeyDTO>> GetAuthKeys()
        {
            _logger.LogInformation("Authorization get auth keys executong...");
            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            if (Request.Headers["key"].Equals(appSettings.GetValue<string>("Secrets:AutorizationKey")))
            {
                return _mapper.Map<List<AuthKeyDTO>>(_autorizationService.GetAuthKeys());
            }

            _logger.LogWarning("Key is invalid.");
            return BadRequest(new { message = "key is invalid" });
        }

        [HttpPost]
        [Route("addAuthKey")]
        [AllowAnonymous]
        public ActionResult<List<AuthKeyDTO>> AddAuthKey([FromBody] List<AuthKeyWithKeyDTO> keys)
        {
            _logger.LogInformation("Authorization add auth key executong...");
            var appSettings = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
            foreach (var key in keys)
            {
                if (key.KeyForAutorization.Equals(appSettings.GetValue<string>("Secrets:AutorizationKey")))
                {
                    var authKey = _mapper.Map<AuthKey>(key);
                    if (!_autorizationService.AddAuthKey(authKey))
                    {
                        _logger.LogWarning("Route already exists.");
                        return BadRequest(new { message = "route already exists" });
                    }
                }
                else
                {
                    _logger.LogWarning("Key is invalid.");
                    return BadRequest(new { message = "key is invalid" });
                }
            }

            return _mapper.Map<List<AuthKeyDTO>>(_autorizationService.GetAuthKeys());
        }
    }
}
