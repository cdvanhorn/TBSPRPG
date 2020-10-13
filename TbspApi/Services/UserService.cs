using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using TbspApi.Entities;
using TbspApi.Models;
using TbspApi.Repositories;
using TbspApi.Utilities;

namespace TbspApi.Services {
    public interface IUserService {
        Task<User> GetById(string id);
        Task<List<User>> GetAll();
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
    }

    public class UserService : IUserService {
        private readonly IJwtSettings _jwtSettings;
        private readonly IDatabaseSettings _databaseSettings;
        private IUserRepository _userRepository;

        public UserService(IJwtSettings jwtSettings, IDatabaseSettings databaseSettings, IUserRepository userRepository) {
            _jwtSettings = jwtSettings;
            _databaseSettings = databaseSettings;
            _userRepository = userRepository;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            //we'll need to add the salt and hash the password
            //then check that against the database value
            string hashedPw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.Password,
                salt: Convert.FromBase64String(_databaseSettings.Salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            var user = await _userRepository.GetUserByUsernameAndPassword(model.Username, hashedPw);
        
            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public Task<User> GetById(string id) {
            return _userRepository.GetUserById(id);
        }

        public Task<List<User>> GetAll() {
            return _userRepository.GetAllUsers();
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}