using SEP.WebShop.Core.Repositories;

namespace SEP.WebShop.Web.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IWebShopUserRepository userRepository, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (!string.IsNullOrWhiteSpace(token))
            {
                var userId = jwtUtils.ValidateToken(token);
                if (userId.HasValue)
                {
                    context.Items["User"] = userRepository.FindById(userId.Value).Value;
                }
            }
            await _next(context);
        }
    }
}
