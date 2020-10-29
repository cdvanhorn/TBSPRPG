using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.Utilities
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

        public async Task Invoke(HttpContext context, IJwtHelper jwtHelper)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, jwtHelper, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, IJwtHelper jwtHelper, string token)
        {
            try
            {
                var userId = jwtHelper.ValidateToken(token);
                // attach user to context on successful jwt validation
                context.Items["UserId"] = userId;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}