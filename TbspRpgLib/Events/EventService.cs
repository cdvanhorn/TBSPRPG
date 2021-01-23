//this will abstract sending events to EventStore,
//so can change to different event store if necessary
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using EventStore.Client;

using TbspRpgLib.Aggregates;

using TbspRpgLib.Settings;

namespace TbspRpgLib.Events
{
    public interface IEventService
    {
        Task SendEvent(Event evnt, bool newStream, ulong expectedStreamPosition);
        void SubscribeByType(string typeName, Action<Event> eventHandler, ulong subscriptionStart);
        Task<List<Event>> GetEventsInStreamAsync(string streamId);
    }

    public class EventService : IEventService {
        private IEventStoreSettings _eventStoreSettings;
        private EventStoreClient _eventStoreClient;

        public EventService(IEventStoreSettings eventStoreSettings) {
            _eventStoreSettings = eventStoreSettings;
            string esUrl = $"{_eventStoreSettings.Url}:{_eventStoreSettings.Port}";
            EventStoreClientSettings settings = new EventStoreClientSettings {
                ConnectivitySettings = {
                    Address = new Uri(esUrl)
                }
            };
            //may be able to put the settings in my config without the eventStoreSettings middleman
            _eventStoreClient = new EventStoreClient(settings);
        }

        public async Task SendEvent(Event evnt, bool newStream, ulong expectedStreamPosition = 0) {
            StreamState state;
            if(newStream) {
                state = StreamState.NoStream;
            } else {
                state = StreamState.Any;
            }
            
            if(expectedStreamPosition == 0) {
                await _eventStoreClient.AppendToStreamAsync(
                    evnt.GetStreamId(),
                    state,
                    new List<EventData> {
                        evnt.ToEventStoreEvent()
                    }
                );
            } else {
                await _eventStoreClient.AppendToStreamAsync(
                    evnt.GetStreamId(),
                    expectedStreamPosition,
                    new List<EventData> {
                        evnt.ToEventStoreEvent()
                    }
                );
            }
        }

        public async void SubscribeByType(string typeName, Action<Event> eventHandler, ulong subscriptionStart) {
            await _eventStoreClient.SubscribeToAllAsync(
                new Position(subscriptionStart, subscriptionStart),
                (subscription, evntdata, token) => {
                    eventHandler(Event.FromEventStoreEvent(evntdata));
                    return Task.CompletedTask;
                },
                filterOptions: new SubscriptionFilterOptions(
	                EventTypeFilter.Prefix(typeName)
                )
            );
        }

        public async Task<List<Event>> GetEventsInStreamAsync(string streamId) {
            var results = _eventStoreClient.ReadStreamAsync(
                Direction.Forwards,
                streamId,
                StreamPosition.Start);
            
            List<Event> events = new List<Event>();
            if(await results.ReadState == ReadState.StreamNotFound) {
                //throw new ArgumentException($"invalid stream id {streamId}");
                return events;
            }
            
            await foreach(var evnt in results) {
                events.Add(Event.FromEventStoreEvent(evnt));
            }
            return events;
        }
    }
}
