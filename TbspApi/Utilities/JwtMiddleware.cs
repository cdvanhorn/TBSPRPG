using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using TbspApi.Services;

namespace TbspApi.Utilities
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJwtSettings _jwtSettings;

        public JwtMiddleware(RequestDelegate next, IJwtSettings jwtSettings)
        {
            _next = next;
            _jwtSettings = jwtSettings;
        }

        public async Task Invoke(HttpContext context, IUserService userService, IJwtHelper jwtHelper)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await attachUserToContext(context, userService, jwtHelper, token);

            await _next(context);
        }

        private async Task attachUserToContext(HttpContext context, IUserService userService, IJwtHelper jwtHelper, string token)
        {
            try
            {
                var userId = jwtHelper.ValidateToken(token);
                // attach user to context on successful jwt validation
                context.Items["User"] = await userService.GetById(userId);
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}