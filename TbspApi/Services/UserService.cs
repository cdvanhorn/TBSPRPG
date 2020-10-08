using System.Collections.Generic;
using System.Linq;
using TbspApi.Entities;
using TbspApi.Models;

namespace TbspApi.Services {
    public interface IUserService {
        IEnumerable<User> GetAll();
        AuthenticateResponse Authenticate(AuthenticateRequest model);
    }

    public class UserService : IUserService {

        private List<User> _users = new List<User>() {
            new User {Id = 1, Username = "test", Password = "test"}
        };

        public UserService() {}

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
        
            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            //var token = generateJwtToken(user);
            var token = "foo";

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll() {
            return _users;
        }
    }
}