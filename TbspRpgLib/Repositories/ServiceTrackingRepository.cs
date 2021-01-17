using System;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using TbspRpgLib.Entities;

namespace TbspRpgLib.Repositories {
    public interface IServiceTrackingRepository {
        Task<EventTypePosition> GetEventTypePosition(Guid serviceId, Guid eventId);
        Task<int> SaveChanges();
        Task AddEventTypePosition(EventTypePosition eventTypePosition);
        Task<ProcessedEvent> GetProcessedEvent(Guid serviceId, Guid eventId);
        Task AddProcessedEvent(ProcessedEvent processedEvent);
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

        public async Task<int> SaveChanges() {
            var updated = await _context.SaveChangesAsync();
            return updated;
        }

        public async Task AddEventTypePosition(EventTypePosition eventTypePosition) {
            var etp = await GetEventTypePosition(eventTypePosition.ServiceId, eventTypePosition.EventTypeId);
            if(etp == null) {
                _context.EventTypePostions.Add(eventTypePosition);
            }
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
    }
}