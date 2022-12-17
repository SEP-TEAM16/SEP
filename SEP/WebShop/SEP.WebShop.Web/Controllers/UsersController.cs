using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SEP.WebShop.Core.Entities;
using SEP.WebShop.Core.Entities.Factories;
using SEP.WebShop.Core.Repositories;
using SEP.WebShop.Core.Services;
using SEP.WebShop.Web.Authorization;
using SEP.WebShop.Web.Dto;
using SEP.WebShop.Web.DtoFactories;

namespace SEP.WebShop.Web.Controllers
{
    [Route("api/users")]
    [Authorize]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IWebShopUserRepository _webShopUserRepository;
        private readonly IJwtUtils _jwtUtils;
        private readonly WebShopUserService _webShopUserService;
        private WebShopUserDtoFactory usersDtoFactory;
        private WebShopUserFactory usersFactory;

        public UsersController(IWebShopUserRepository webShopUserRepository, WebShopUserService webShopUserService, IJwtUtils jwtUtils)
        {
            _webShopUserRepository = webShopUserRepository;
            _jwtUtils = jwtUtils;
            _webShopUserService = webShopUserService;
            usersDtoFactory = new WebShopUserDtoFactory();
            usersFactory = new WebShopUserFactory();
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult Authenticate(AuthenticationRequest authenticationRequest)
        {

            Maybe<WebShopUser> user = _webShopUserRepository.FindByUsername(authenticationRequest.Username);

            if (user.HasNoValue)
                return BadRequest(new { message = "User with specified username does not exists" });

            bool isPasswordVerified = user.Value.Password.Verify(authenticationRequest.Password);

            if (!isPasswordVerified)
            {
                return BadRequest(new { message = "Password you entered does not match users password" });
            }

            var response = new AuthenticationResponse(user.Value, _jwtUtils.GenerateToken(user.Value));

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Register(WebShopUserDto user)
        {
            Result<WebShopUser> result = usersFactory.Create(Guid.NewGuid(), user.Username, user.Password, user.EmailAddress, user.Name, user.City, user.Street, user.UserType);
            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }
            Result createUserResult = _webShopUserService.Create(result.Value);

            if (createUserResult.IsFailure)
                return BadRequest(createUserResult.Error);

            WebShopUserDto createdUser = usersDtoFactory.Create(result.Value);

            return Created("api/users/" + result.Value.Id.ToString(), createdUser);

        }

    }
}
