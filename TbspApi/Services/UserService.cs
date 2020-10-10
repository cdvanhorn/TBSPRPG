using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using TbspApi.Entities;
using TbspApi.Models;
using TbspApi.Utilities;

namespace TbspApi.Services {
    public interface IUserService {
        User GetById(int id);
        IEnumerable<User> GetAll();
        AuthenticateResponse Authenticate(AuthenticateRequest model);
    }

    public class UserService : IUserService {

        private List<User> _users = new List<User>() {
            new User {Id = 1, Username = "test", Password = "test"}
        };

        private readonly JwtSettings _jwtSettings;
        private readonly DatabaseSettings _databaseSettings;

        public UserService(IOptions<JwtSettings> jwtSettings, IOptions<DatabaseSettings> databaseSettings) {
            _jwtSettings = jwtSettings.Value;
            _databaseSettings = databaseSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            //we'll need to add the salt and hash the password
            //then check that against the database value
            string hashedPw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: model.Password,
                salt: Convert.FromBase64String(_databaseSettings.Salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        
            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }


//var client = new MongoClient("mongodb+srv://TbspApiAdmin:<password>@tbsprpgdev.zqgsk.mongodb.net/<dbname>?retryWrites=true&w=majority");
//var database = client.GetDatabase("test");


        public User GetById(int id) {
            return _users.Where(u => u.Id == id).FirstOrDefault();
        }

        public IEnumerable<User> GetAll() {
            return _users;
        }

        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.secret);
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