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
        Task SendEvent(Event evnt, bool newStream);
        Task SendEvent(Event evnt, ulong expectedStreamPosition);
        void SubscribeByType(string typeName, Func<Event, Task> eventHandler, ulong subscriptionStart);
        Task<List<Event>> GetEventsInStreamAsync(string streamId);
    }

    public class EventService : IEventService {
        private IEventStoreSettings _eventStoreSettings;
        private EventStoreClient _eventStoreClient;

        private int _retries = 5;
        private int _tries = 0;

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

        public async Task SendEvent(Event evnt, bool newStream) {
            StreamState state;
            if(newStream) {
                state = StreamState.NoStream;
            } else {
                state = StreamState.Any;
            }
            
            await _eventStoreClient.AppendToStreamAsync(
                evnt.GetStreamId(),
                state,
                new List<EventData> {
                    evnt.ToEventStoreEvent()
                }
            );
        }

        public async Task SendEvent(Event evnt, ulong expectedStreamPosition) {   
            await _eventStoreClient.AppendToStreamAsync(
                evnt.GetStreamId(),
                expectedStreamPosition,
                new List<EventData> {
                    evnt.ToEventStoreEvent()
                }
            );
        }

        public async void SubscribeByType(string typeName, Func<Event, Task> eventHandler, ulong subscriptionStart) {
            var checkpoint = new Position(subscriptionStart, subscriptionStart);
            await _eventStoreClient.SubscribeToAllAsync(
                checkpoint,
                eventAppeared: async (subscription, evntdata, token) => {
                    await eventHandler(Event.FromEventStoreEvent(evntdata));
                    checkpoint = evntdata.OriginalPosition.Value;
                },
                subscriptionDropped: ((subscription, reason, exception) => {
		            Console.WriteLine($"subscription was dropped due to {reason}.");
                    if(reason != SubscriptionDroppedReason.Disposed && _tries < _retries) {
                        _tries++;
                        SubscribeByType(typeName, eventHandler, checkpoint.PreparePosition);
                    }
                }),
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
