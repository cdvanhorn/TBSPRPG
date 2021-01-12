using System;
using System.Threading.Tasks;

using TbspRpgLib.Entities;
using TbspRpgLib.Repositories;

namespace TbspRpgLib.Services {
    public interface IServiceTrackingService {
        void UpdatePosition(Guid serviceId, Guid eventTypeId, ulong position);
        Task<ulong> GetPosition(Guid serviceId, Guid eventTypeId);
        Task<bool> HasBeenProcessed(Guid serviceId, Guid eventId);
        void EventProcessed(Guid serviceId, Guid eventId);
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
            if(etp != null && etp.Position < position) {
                etp.Position = position;
                _serviceTrackingRepository.SaveChanges();
            } else if(etp == null){
                _serviceTrackingRepository.InsertEventTypePosition(new EventTypePosition() {
                    ServiceId = serviceId,
                    EventTypeId = eventTypeId,
                    Position = position
                });
            }
        }

        public async Task<bool> HasBeenProcessed(Guid serviceId, Guid eventId) {
            var pe = await _serviceTrackingRepository.GetProcessedEvent(serviceId, eventId);
            if(pe == null)
                return false;
            return true;
        }

        public void EventProcessed(Guid serviceId, Guid eventId) {
            _serviceTrackingRepository.InsertProcessedEvent(new ProcessedEvent() {
                ServiceId = serviceId,
                EventId = eventId
            });
        }
    }
}