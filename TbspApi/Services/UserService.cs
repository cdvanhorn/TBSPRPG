using System.Collections.Generic;
using TbspApi.Entities;

namespace TbspApi.Services {
    public interface IUserService {
        IEnumerable<User> GetAll();
    }

    public class UserService : IUserService {

        private List<User> _users = new List<User>() {
            new User {Id = 1, UserName = "test", Password = "test"}
        };

        public UserService() {}

        public IEnumerable<User> GetAll() {
            return _users;
        }
    }
}