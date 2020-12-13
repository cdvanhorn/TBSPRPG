using System;
using System.Threading.Tasks;

using TbspRpgLib.Events;

namespace TbspRpgLib.Aggregates {
    public interface IAggregateService {
        Task<Aggregate> BuildAggregate(string aggregateId, string aggregateTypeName);
        void SubscribeByType(string typeName, Action<Aggregate, string, ulong> eventHandler, string servicePrefix, ulong subscriptionStart = 0);
    }

    public class AggregateService : IAggregateService {
        private IEventService _eventService;

        public AggregateService(IEventService eventService) {
            _eventService = eventService;
        }

        public void SubscribeByType(string typeName, Action<Aggregate, string, ulong> eventHandler, string servicePrefix, ulong subscriptionStart = 0) {
            _eventService.SubscribeByType(
                typeName,
                (evnt) => {
                    HandleEvent(evnt, eventHandler, servicePrefix);
                },
                subscriptionStart
            );
        }

        public async void HandleEvent(Event evnt, Action<Aggregate, string, ulong> eventHandler, string servicePrefix) {
            //check if the aggregate id is ok, produce an aggregate
            var aggregateId = evnt.GetStreamId();
            if(aggregateId == null) //we can't parse this event
                return;

            //check if the event id prefixed with the service id exists in the list of processed ids in the aggregrate
            var aggregate = await BuildAggregate(aggregateId, "GameAggregate");
            //only call the event handler if we haven't processed this event
            var processedId = $"{servicePrefix}_{evnt.EventId}";
            if(!aggregate.ProcessedEventIds.Contains(processedId))
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
                if(!string.IsNullOrEmpty(evnt.GetProcessedEventId())) {
                    aggregate.ProcessedEventIds.Add(evnt.GetProcessedEventId());
                }
            }
            return aggregate;
        }
    }
}
