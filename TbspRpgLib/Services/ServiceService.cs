using System.Threading.Tasks;
using System.Collections.Generic;

using TbspRpgLib.Repositories;
using TbspRpgLib.Entities;

namespace TbspRpgLib.Services {
    public interface IServiceService {
        string GetUrlForService(string name);
        Service GetServiceByName(string name);
        List<Service> GetAllServices();
        //void UpdateService(Service service, string eventName);
    }

    public class ServiceService : IServiceService{
        public const string GAME_SERVICE_NAME = "game";
        public const string MAP_SERVICE_NAME = "map";
        public const string ADVENTURE_SERVICE_NAME = "adventure";
        
        private IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository) {
            _serviceRepository = serviceRepository;
        }

        public List<Service> GetAllServices() {
            return _serviceRepository.GetAllServices();
        }

        public Service GetServiceByName(string name) {
            return _serviceRepository.GetServiceByName(name);
        }

        // public void UpdateService(Service service, string eventName) {
        //     _serviceRepository.UpdateService(service, eventName);
        // }

        public string GetUrlForService(string name) {
            var service = GetServiceByName(name);
            return service.Url;
        }
    }
}