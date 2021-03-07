using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using TbspRpgLib.Events;
using TbspRpgLib.Repositories;
using TbspRpgLib.Services;

namespace TbspRpgLib.Aggregates {
    public interface IAggregateService {
        Task<Aggregate> BuildAggregate(string aggregateId, string aggregateTypeName);
        Task<Aggregate> BuildPartialAggregate(
            string aggregateId,
            string aggregateTypeName,
            ulong start);
        Task<Aggregate> BuildPartialAggregate(
            string aggregateId,
            string aggregateTypeName,
            ulong start,
            long count);
        Task<Aggregate> BuildPartialAggregateReverse(
            string aggregateId,
            string aggregateTypeName,
            ulong start);
        Task<Aggregate> BuildPartialAggregateReverse(
            string aggregateId,
            string aggregateTypeName,
            ulong start,
            long count);
        Task<Aggregate> BuildPartialAggregateLatest(string aggregateId, string aggregateTypeName);
        void SubscribeByType(string typeName, Func<Aggregate, Event, Task> eventHandler, ulong subscriptionStart = 0);
        Task AppendToAggregate(string aggregateTypeName, Event evnt, ulong expectedPosition);
        Task AppendToAggregate(string aggregateTypeName, Event evnt, bool newStream);
    }

    public class AggregateService : IAggregateService {
        public const string CONTENT_AGGREGATE_TYPE = "ContentAggregate";
        public const string GAME_AGGREGATE_TYPE = "GameAggregate";
        private IEventService _eventService;
        private IAggregateTypeService _aggregateTypeService;

        public AggregateService(IEventService eventService, IAggregateTypeService aggregateTypeService) {
            _eventService = eventService;
            _aggregateTypeService = aggregateTypeService;
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
            var aggregateId = evnt.StreamId;
            if(aggregateId == null) //we can't parse this event
                return;

            string aggregateTypeName = _aggregateTypeService.GetAggregateTypeName(aggregateId);

            //build the aggregate
            var aggregate = await BuildAggregate(aggregateId, aggregateTypeName);
            aggregate.GlobalPosition = evnt.GlobalPosition;
            await eventHandler(aggregate, evnt);
        }

        private Aggregate CreateAggregate(string aggregateTypeName) {
            string fqname = $"TbspRpgLib.Aggregates.{aggregateTypeName}";
            Type aggregateType = Type.GetType(fqname);
            if(aggregateType == null)
                throw new ArgumentException($"invalid aggregate type name {aggregateTypeName}");
            Aggregate aggregate = (Aggregate)Activator.CreateInstance(aggregateType);
            return aggregate;
        }

        private void CompileAggregate(Aggregate aggregate, List<Event> events) {
            foreach(var evnt in events) {
                evnt.UpdateAggregate(aggregate);
                if(evnt.StreamPosition > aggregate.StreamPosition)
                    aggregate.StreamPosition = evnt.StreamPosition;
            }
        }

        public async Task<Aggregate> BuildAggregate(string aggregateId, string aggregateTypeName) {
            //create a new aggregate of the appropriate type
            Aggregate aggregate = CreateAggregate(aggregateTypeName);
            //get all of the events in the aggregrate id stream
            var events = await _eventService.GetAllEventsInStreamAsync(
                PrepareAggregateId(aggregateId, aggregateTypeName)
            );
            CompileAggregate(aggregate, events);
            return aggregate;
        }

        public async Task<Aggregate> BuildPartialAggregate(
            string aggregateId,
            string aggregateTypeName,
            ulong start)
        {
            //create a new aggregate of the appropriate type
            Aggregate aggregate = CreateAggregate(aggregateTypeName);
            //get all of the events in the aggregrate id stream
            var events = await _eventService.GetEventsInStreamAsync(
                PrepareAggregateId(aggregateId, aggregateTypeName),
                start
            );
            CompileAggregate(aggregate, events);
            return aggregate;
        }

        public async Task<Aggregate> BuildPartialAggregate(
            string aggregateId,
            string aggregateTypeName,
            ulong start,
            long count)
        {
            //create a new aggregate of the appropriate type
            Aggregate aggregate = CreateAggregate(aggregateTypeName);
            //get all of the events in the aggregrate id stream
            var events = await _eventService.GetEventsInStreamAsync(
                PrepareAggregateId(aggregateId, aggregateTypeName),
                start,
                count
            );
            CompileAggregate(aggregate, events);
            return aggregate;
        }

        public async Task<Aggregate> BuildPartialAggregateReverse(
            string aggregateId,
            string aggregateTypeName,
            ulong start)
        {
            //create a new aggregate of the appropriate type
            Aggregate aggregate = CreateAggregate(aggregateTypeName);
            //get all of the events in the aggregrate id stream
            var events = await _eventService.GetEventsInStreamReverseAsync(
                PrepareAggregateId(aggregateId, aggregateTypeName),
                start
            );
            CompileAggregate(aggregate, events);
            return aggregate;
        }

        public async Task<Aggregate> BuildPartialAggregateReverse(
            string aggregateId,
            string aggregateTypeName,
            ulong start,
            long count)
        {
            //create a new aggregate of the appropriate type
            Aggregate aggregate = CreateAggregate(aggregateTypeName);
            //get all of the events in the aggregrate id stream
            var events = await _eventService.GetEventsInStreamReverseAsync(
                PrepareAggregateId(aggregateId, aggregateTypeName),
                start,
                count
            );
            CompileAggregate(aggregate, events);
            return aggregate;
        }

        public async Task<Aggregate> BuildPartialAggregateLatest(string aggregateId, string aggregateTypeName)
        {
            //create a new aggregate of the appropriate type
            Aggregate aggregate = CreateAggregate(aggregateTypeName);
            //get all of the events in the aggregrate id stream
            var evnt = await _eventService.GetLatestEventInStreamAsync(
                PrepareAggregateId(aggregateId, aggregateTypeName)
            );
            evnt.UpdateAggregate(aggregate);
            aggregate.StreamPosition = evnt.StreamPosition;
            return aggregate;
        }

        private string PrepareAggregateId(string aggregateId, string aggregateTypeName) {
            if(_aggregateTypeService.IsIdAlreadyPrefixed(aggregateId, aggregateTypeName))
                return aggregateId;
            else
                return _aggregateTypeService.GenerateAggregateIdForAggregateType(
                        aggregateId,
                        aggregateTypeName
                    );
        }

        public async Task AppendToAggregate(string aggregateTypeName, Event evnt, bool newStream) {
            PrepareEventForAppend(aggregateTypeName, evnt);
            await _eventService.SendEvent(evnt, newStream);
        }

        public async Task AppendToAggregate(string aggregateTypeName, Event evnt, ulong expectedPosition) {
            PrepareEventForAppend(aggregateTypeName, evnt);
            await _eventService.SendEvent(evnt, expectedPosition);
        }

        private void PrepareEventForAppend(string aggregateTypeName, Event evnt) {
            if(evnt.StreamId == null) {
                //we need to populate the stream id
                evnt.StreamId = 
                    _aggregateTypeService.GenerateAggregateIdForAggregateType(
                        evnt.GetDataId(),
                        aggregateTypeName
                    );
            }
        }
    }
}
