using System;
using System.Threading.Tasks;

using TbspRpgLib.Entities;
using TbspRpgLib.Repositories;

namespace TbspRpgLib.Services {
    public interface IServiceTrackingService {
        Task UpdatePosition(Guid eventTypeId, ulong position);
        Task<ulong> GetPosition(Guid eventTypeId);
        Task<bool> HasBeenProcessed(Guid eventId);
        Task EventProcessed(Guid eventId);
    }

    public class ServiceTrackingService : IServiceTrackingService {
        private IServiceTrackingRepository _serviceTrackingRepository;

        public ServiceTrackingService(IServiceTrackingRepository serviceTrackingRepository) {
            _serviceTrackingRepository = serviceTrackingRepository;
        }

        public async Task<ulong> GetPosition(Guid eventTypeId) {
            var etp = await _serviceTrackingRepository.GetEventTypePosition(eventTypeId);
            if(etp != null)
                return etp.Position;
            return 0;
        }

        public async Task UpdatePosition(Guid eventTypeId, ulong position) {
            var etp = await _serviceTrackingRepository.GetEventTypePosition(eventTypeId);
            if(etp != null && etp.Position < position) {
                etp.Position = position;
            } else if(etp == null){
                await _serviceTrackingRepository.AddEventTypePosition(new EventTypePosition() {
                    Id = Guid.NewGuid(),
                    EventTypeId = eventTypeId,
                    Position = position
                });
            }
        }

        public async Task<bool> HasBeenProcessed(Guid eventId) {
            var pe = await _serviceTrackingRepository.GetProcessedEvent(eventId);
            if(pe == null)
                return false;
            return true;
        }

        public async Task EventProcessed(Guid eventId) {
            await _serviceTrackingRepository.AddProcessedEvent(new ProcessedEvent() {
                Id = Guid.NewGuid(),
                EventId = eventId
            });
        }
    }
}