using System;
using System.Threading.Tasks;

using TbspRpgLib.Events;
using TbspRpgLib.Services;

namespace TbspRpgLib.Aggregates {
    public interface IAggregateService {
        Task<Aggregate> BuildAggregate(string aggregateId, string aggregateTypeName);
        void SubscribeByType(string typeName, Func<Aggregate, Event, Task> eventHandler, ulong subscriptionStart = 0);
    }

    public class AggregateService : IAggregateService {
        private IEventService _eventService;

        public AggregateService(IEventService eventService) {
            _eventService = eventService;
        }

        public void SubscribeByType(string typeName, Func<Aggregate, Event, Task> eventHandler, ulong subscriptionStart = 0) {
            _eventService.SubscribeByType(
                typeName,
                async (evnt) => {
                    await HandleEvent(evnt, eventHandler);
                },
                subscriptionStart
            );
        }

        public async Task HandleEvent(Event evnt, Func<Aggregate, Event, Task> eventHandler) {
            //check if the aggregate id is ok, produce an aggregate
            var aggregateId = evnt.GetStreamId();
            if(aggregateId == null) //we can't parse this event
                return;

            //build the aggregate
            var aggregate = await BuildAggregate(aggregateId, "GameAggregate");
            aggregate.GlobalPosition = evnt.GlobalPosition;
            await eventHandler(aggregate, evnt);
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
                if(evnt.StreamPosition > aggregate.StreamPosition)
                    aggregate.StreamPosition = evnt.StreamPosition;
            }
            return aggregate;
        }
    }
}
