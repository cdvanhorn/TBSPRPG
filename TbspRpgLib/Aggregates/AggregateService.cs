using System;
using System.Threading.Tasks;

using TbspRpgLib.Events;
using TbspRpgLib.Services;

namespace TbspRpgLib.Aggregates {
    public interface IAggregateService {
        Task<Aggregate> BuildAggregate(string aggregateId, string aggregateTypeName);
        void SubscribeByType(string typeName, Action<Aggregate, string, ulong> eventHandler, Guid serviceId, ulong subscriptionStart = 0);
    }

    public class AggregateService : IAggregateService {
        private IEventService _eventService;
        private IServiceTrackingService _serviceTrackingService;

        public AggregateService(IEventService eventService, IServiceTrackingService serviceTrackingService) {
            _eventService = eventService;
            _serviceTrackingService = serviceTrackingService;
        }

        public void SubscribeByType(string typeName, Action<Aggregate, string, ulong> eventHandler, Guid serviceId, ulong subscriptionStart = 0) {
            _eventService.SubscribeByType(
                typeName,
                (evnt) => {
                    HandleEvent(evnt, eventHandler, serviceId);
                },
                subscriptionStart
            );
        }

        public async void HandleEvent(Event evnt, Action<Aggregate, string, ulong> eventHandler, Guid serviceId) {
            //check if the aggregate id is ok, produce an aggregate
            var aggregateId = evnt.GetStreamId();
            if(aggregateId == null) //we can't parse this event
                return;

            //build the aggregate
            var aggregate = await BuildAggregate(aggregateId, "GameAggregate");
            if(!await _serviceTrackingService.HasBeenProcessed(serviceId, evnt.EventId))
                eventHandler(aggregate, evnt.EventId.ToString(), evnt.Position);
        }

        public async Task<Aggregate> BuildAggregate(string aggregateId, string aggregateTypeName) {
            //create a new aggregate of the appropriate type
            string fqname = $"TbspRpgLib.Aggregates.{aggregateTypeName}";
            Type aggregateType = Type.GetType(fqname);
            if(aggregateType == null)
                throw new ArgumentException($"invalid aggregate type name {aggregateTypeName}");
            Aggregate aggregate = (Aggregate)Activator.CreateInstance(aggregateType);

            //get all of the events in the aggregrate id stream
            var events = await _eventService.GetEventsInStreamAsync(aggregateId);
            foreach(var evnt in events) {
                evnt.UpdateAggregate(aggregate);
            }
            return aggregate;
        }
    }
}
