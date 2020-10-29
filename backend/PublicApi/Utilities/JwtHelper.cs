using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PublicApi.Utilities {
    public interface IJwtHelper {
        string GenerateToken(string userId);
        string ValidateToken(string token);
    }

    public class JwtHelper : IJwtHelper{
        private readonly IJwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        public JwtHelper(IJwtSettings jwtSettings) {
            _jwtSettings = jwtSettings;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string ValidateToken(string token) {
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            _tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.First(x => x.Type == "id").Value;
        }

        public string GenerateToken(string userId) {
            // generate token that is valid for 7 days
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userId) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }
    }
}