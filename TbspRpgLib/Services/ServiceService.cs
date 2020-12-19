using System.Threading.Tasks;
using System.Collections.Generic;

using TbspRpgLib.Repositories;
using TbspRpgLib.Entities;

namespace TbspRpgLib.Services {
    public interface IServiceService {
        Task<string> GetUrlForService(string name);
        Task<Service> GetServiceByName(string name);
        Task<List<Service>> GetAllServices();
        void UpdateService(Service service, string eventName);
    }

    public class ServiceService : IServiceService{
        private IServiceRepository _serviceRepository;

        public ServiceService(IServiceRepository serviceRepository) {
            _serviceRepository = serviceRepository;
        }

        public Task<List<Service>> GetAllServices() {
            return _serviceRepository.GetAllServices();
        }

        public Task<Service> GetServiceByName(string name) {
            return _serviceRepository.GetServiceByName(name);
        }

        public void UpdateService(Service service, string eventName) {
            _serviceRepository.UpdateService(service, eventName);
        }

        public async Task<string> GetUrlForService(string name) {
            var service = await GetServiceByName(name);
            return service.Url;
        }
    }
}