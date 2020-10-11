using Microsoft.Extensions.Options;

using MongoDB.Driver;

using System;
using System.Collections.Generic;

using TbspApi.Entities;
using TbspApi.Utilities;

namespace TbspApi.Repositories {
    public interface IUserRepository {
        User GetUserById(string id);
        IEnumerable<User> GetAllUsers();
        User GetUserByUsernameAndPassword(string username, string password);
    }

    public class UserRepository : IUserRepository{
        private readonly DatabaseSettings _dbSettings;
        private readonly IMongoCollection<User> _users;

        public UserRepository(IOptions<DatabaseSettings> databaseSettings) {
            _dbSettings = databaseSettings.Value;

            var connectionString = $"mongodb+srv://{_dbSettings.Username}:{_dbSettings.Password}@{_dbSettings.Url}/{_dbSettings.Name}?retryWrites=true&w=majority";
            Console.WriteLine(connectionString);
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(_dbSettings.Name);

            _users = database.GetCollection<User>("users");
        }

        public User GetUserById(string id) {
            return _users.Find<User>(u => u.Id == id).FirstOrDefault();
        }

        public User GetUserByUsernameAndPassword(string username, string password) {
            return _users.Find<User>(user => user.Username == username && user.Password == password).FirstOrDefault();
        }

        public IEnumerable<User> GetAllUsers() {
            return _users.Find(user => true).ToList();
        }
    }
}