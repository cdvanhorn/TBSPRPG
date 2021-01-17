using System;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using TbspRpgLib.Entities;

namespace TbspRpgLib.Repositories {
    public interface IServiceTrackingRepository {
        Task<EventTypePosition> GetEventTypePosition(Guid serviceId, Guid eventId);
        Task AddEventTypePosition(EventTypePosition eventTypePosition);
        Task RemoveAllEventTypePositions();
        Task<ProcessedEvent> GetProcessedEvent(Guid serviceId, Guid eventId);
        Task AddProcessedEvent(ProcessedEvent processedEvent);
        Task RemoveAllProcessedEvents();
    }

    public class ServiceTrackingRepository : IServiceTrackingRepository{
        private ServiceTrackingContext _context;

        public ServiceTrackingRepository(ServiceTrackingContext context) {
            _context = context;
        }

        public Task<EventTypePosition> GetEventTypePosition(Guid serviceId, Guid eventId) {
            return _context.EventTypePostions.AsQueryable()
                    .Where(etp => etp.ServiceId == serviceId)
                    .Where(etp => etp.EventTypeId == eventId)
                    .FirstOrDefaultAsync();
        }

        public async Task AddEventTypePosition(EventTypePosition eventTypePosition) {
            var etp = await GetEventTypePosition(eventTypePosition.ServiceId, eventTypePosition.EventTypeId);
            if(etp == null) {
                _context.EventTypePostions.Add(eventTypePosition);
            }
        }

        public async Task RemoveAllEventTypePositions() {
            var etps = await _context.EventTypePostions.AsQueryable().ToListAsync();
            _context.EventTypePostions.RemoveRange(etps);
        }

        public Task<ProcessedEvent> GetProcessedEvent(Guid serviceId, Guid eventId) {
            return _context.ProcessedEvents.AsQueryable()
                    .Where(pe => pe.ServiceId == serviceId)
                    .Where(pe => pe.EventId == eventId)
                    .FirstOrDefaultAsync();
        }

        public async Task AddProcessedEvent(ProcessedEvent processedEvent) {
            var pe = await GetProcessedEvent(processedEvent.ServiceId, processedEvent.EventId);
            if(pe == null) {
                _context.ProcessedEvents.Add(processedEvent);
            }
        }

        public async Task RemoveAllProcessedEvents() {
            var pes = await _context.ProcessedEvents.AsQueryable().ToListAsync();
            _context.ProcessedEvents.RemoveRange(pes);
        }
    }
}