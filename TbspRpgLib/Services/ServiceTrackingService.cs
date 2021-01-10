using System;
using System.Threading.Tasks;

using TbspRpgLib.Entities;
using TbspRpgLib.Repositories;

namespace TbspRpgLib.Services {
    public interface IServiceTrackingService {
        void UpdatePosition(Guid serviceId, Guid eventTypeId, ulong position);
        Task<ulong> GetPosition(Guid serviceId, Guid eventTypeId);
    }

    public class ServiceTrackingService : IServiceTrackingService {
        private IServiceTrackingRepository _serviceTrackingRepository;

        public ServiceTrackingService(IServiceTrackingRepository serviceTrackingRepository) {
            _serviceTrackingRepository = serviceTrackingRepository;
        }

        public async Task<ulong> GetPosition(Guid serviceId, Guid eventTypeId) {
            var etp = await _serviceTrackingRepository.GetEventTypePosition(serviceId, eventTypeId);
            if(etp != null)
                return etp.Position;
            return 0;
        }

        public async void UpdatePosition(Guid serviceId, Guid eventTypeId, ulong position) {
            var etp = await _serviceTrackingRepository.GetEventTypePosition(serviceId, eventTypeId);
            if(etp != null) {
                etp.Position = position;
                _serviceTrackingRepository.SaveChanges();
            } else {
                _serviceTrackingRepository.InsertEventTypePosition(new EventTypePosition() {
                    ServiceId = serviceId,
                    EventTypeId = eventTypeId,
                    Position = position
                });
            }
        }
    }
}