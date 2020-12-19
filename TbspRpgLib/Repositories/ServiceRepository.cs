using TbspRpgLib.Entities;

using MongoDB.Driver;

using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

using TbspRpgLib.Settings;

namespace TbspRpgLib.Repositories {
    public interface IServiceRepository {
        Task<List<Service>> GetAllServices();

        Task<Service> GetServiceByName(string name);

        void UpdateService(Service service, string eventName);
    }

    public class ServiceRepository : MongoRepository, IServiceRepository {
        private readonly IMongoCollection<Service> _services;

        public ServiceRepository(IDatabaseSettings databaseSettings) : base(databaseSettings) {
            _services = _mongoDatabase.GetCollection<Service>("services");
        }

        public Task<List<Service>> GetAllServices() {
            return _services.Find(service => true).ToListAsync();
        }

        public Task<Service> GetServiceByName(string name) {
            return _services.Find(service => 
                name.ToLower() == service.Name.ToLower()).FirstOrDefaultAsync();
        }

        public void UpdateService(Service service, string eventName) {
            var newPosition = service.EventIndexes.Where(ei => ei.EventName == eventName).First().Index;
            var filter = Builders<Service>.Filter.Eq(doc => doc.Id, service.Id)
                & Builders<Service>.Filter.ElemMatch(doc => doc.EventIndexes, 
                    Builders<EventIndex>.Filter.Eq(ei => ei.EventName, eventName)
                    & Builders<EventIndex>.Filter.Lt(ei => ei.Index, newPosition));
            
            var update = Builders<Service>.Update.Set(
                doc => doc.EventIndexes[-1].Index,
                newPosition
            );
            var result = _services.UpdateOneAsync(filter, update);
        }
    }
}
