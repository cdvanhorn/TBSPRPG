using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using TbspApi.Entities;

namespace TbspApi.Utilities {
    public interface IJwtHelper {
        string GenerateToken(User user);
    }

    public class JwtHelper : IJwtHelper{
        private readonly IJwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        public JwtHelper(IJwtSettings jwtSettings) {
            _jwtSettings = jwtSettings;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateToken(User user) {
            // generate token that is valid for 7 days
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }
    }
}