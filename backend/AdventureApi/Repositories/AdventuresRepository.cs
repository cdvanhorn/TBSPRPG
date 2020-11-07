using AdventureApi.Entities;

using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AdventureApi.Utilities;

namespace AdventureApi.Repositories {
    public interface IAdventuresRepository {
        Task<List<Adventure>> GetAllAdventures();
        Task<Adventure> GetAdventureByName(string name);
    }

    public class AdventuresRepository : IAdventuresRepository {
        private readonly IDatabaseSettings _dbSettings;

        private readonly IMongoCollection<Adventure> _adventures;

        public AdventuresRepository(IDatabaseSettings databaseSettings) {
            _dbSettings = databaseSettings;

            var connectionString = $"mongodb+srv://{_dbSettings.Username}:{_dbSettings.Password}@{_dbSettings.Url}/{_dbSettings.Name}?retryWrites=true&w=majority";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(_dbSettings.Name);

            _adventures = database.GetCollection<Adventure>("adventures");
        }

        public Task<List<Adventure>> GetAllAdventures() {
            return _adventures.Find(adventure => true).ToListAsync();
        }

        public Task<Adventure> GetAdventureByName(string name) {
            return _adventures.Find(adventure => adventure.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
        }
    }
}