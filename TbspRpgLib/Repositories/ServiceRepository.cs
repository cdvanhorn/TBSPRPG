using TbspRpgLib.Entities;

using Microsoft.EntityFrameworkCore;

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

    public class ServiceRepository : IServiceRepository {

        public ServiceRepository() {

        }

        public Task<List<Service>> GetAllServices() {
            //return _services.Find(service => true).ToListAsync();
            return null;
        }

        public Task<Service> GetServiceByName(string name) {
            // return _services.Find(service => 
            //     name.ToLower() == service.Name.ToLower()).FirstOrDefaultAsync();
            return null;
        }

        public void UpdateService(Service service, string eventName) {
            // var newPosition = service.EventIndexes.Where(ei => ei.EventName == eventName).First().Index;
            // FilterDefinition<Service> idFilter = Builders<Service>.Filter.Eq(doc => doc.Id, service.Id);
            // FilterDefinition<Service> indexPositionFilter = Builders<Service>.Filter.ElemMatch(doc => doc.EventIndexes, 
            //         Builders<EventIndex>.Filter.Eq(ei => ei.EventName, eventName)
            //         & Builders<EventIndex>.Filter.Lt(ei => ei.Index, newPosition));
            // var filter = Builders<Service>.Filter.And(idFilter, indexPositionFilter);
            
            
            // var update = Builders<Service>.Update.Set(
            //     doc => doc.EventIndexes[-1].Index,
            //     newPosition
            // );
            
            // var result = _services.UpdateOneAsync(filter, update);
        }
    }
}
