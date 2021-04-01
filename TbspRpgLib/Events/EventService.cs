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
        Task<List<Event>> GetAllEventsInStreamAsync(string streamId);
        Task<List<Event>> GetAllEventsInStreamReverseAsync(string streamId);
        Task<Event> GetLatestEventInStreamAsync(string streamId);
        Task<Event> GetEventInStreamAsync(string streamId, ulong index);
        Task<List<Event>> GetEventsInStreamAsync(string streamId, ulong start);
        Task<List<Event>> GetEventsInStreamAsync(string streamId, ulong start, long count);
        Task<List<Event>> GetEventsInStreamReverseAsync(string streamId, long start);
        Task<List<Event>> GetEventsInStreamReverseAsync(string streamId, long start, long count);
    }

    internal class EventService : IEventService {
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
                evnt.StreamId,
                state,
                new List<EventData> {
                    evnt.ToEventStoreEvent()
                }
            );
        }

        public async Task SendEvent(Event evnt, ulong expectedStreamPosition) {   
            await _eventStoreClient.AppendToStreamAsync(
                evnt.StreamId,
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
                        //wait a half second before resubscribing
                        Thread.Sleep(500);
                        _tries++;
                        SubscribeByType(typeName, eventHandler, checkpoint.PreparePosition);
                    }
                }),
                filterOptions: new SubscriptionFilterOptions(
	                EventTypeFilter.Prefix(typeName)
                )
            );
        }

        //get all of the events in the stream in order
        public async Task<List<Event>> GetAllEventsInStreamAsync(string streamId) {
            return await GetEventsInStreamAsync(
                streamId,
                Direction.Forwards,
                0
            );
        }

        //get all of the events in the stream in reverse
        public async Task<List<Event>> GetAllEventsInStreamReverseAsync(string streamId) {
            return await GetEventsInStreamAsync(
                streamId,
                Direction.Backwards,
                -1
            );
        }

        //Get the latest event in the stream
        public async Task<Event> GetLatestEventInStreamAsync(string streamId) {
            var events = await GetEventsInStreamAsync(
                streamId,
                Direction.Backwards,
                -1,
                1
            );
            if(events.Count > 0)
                return events[0];
            return null;
        }

        public async Task<Event> GetEventInStreamAsync(string streamId, ulong index) {
            var events = await GetEventsInStreamAsync(
                streamId,
                Direction.Forwards,
                (long)index,
                1
            );
            if(events.Count > 0)
                return events[0];
            return null;
        }

        //get the events in order from the specified position to the end
        public async Task<List<Event>> GetEventsInStreamAsync(string streamId, ulong start) {
            return await GetEventsInStreamAsync(
                streamId,
                Direction.Forwards,
                (long)start
            );
        }

        //get the specified number of events in order from the specified position
        public async Task<List<Event>> GetEventsInStreamAsync(string streamId, ulong start, long count) {
            return await GetEventsInStreamAsync(
                streamId,
                Direction.Forwards,
                (long)start,
                count
            );
        }

        //get the events in reverse from the specified position to the beginning
        public async Task<List<Event>> GetEventsInStreamReverseAsync(string streamId, long start) {
            return await GetEventsInStreamAsync(
                streamId,
                Direction.Backwards,
                start
            );
        }

        //get the specified number of events in reverse from the specified position
        public async Task<List<Event>> GetEventsInStreamReverseAsync(string streamId, long start, long count) {
            return await GetEventsInStreamAsync(
                streamId,
                Direction.Backwards,
                start,
                count
            );
        }

        //complex method used by easier methods
        //if start is 0 will start at the beginning of the stream
        //if start > 0 will use that as the start position
        //if start < 0 will start at the end of the stream
        //if count is > 0 will retrieve at most that number of events
        //if count is 0 will read all events
        private async Task<List<Event>> GetEventsInStreamAsync(string streamId, Direction direction, long start, long count = 0) {
            EventStoreClient.ReadStreamResult results = null;
            if(start == 0 && count > 0) {
                results = _eventStoreClient.ReadStreamAsync(
                    direction,
                    streamId,
                    StreamPosition.Start,
                    count
                );
            } else if(start == 0 && count == 0) {
                results = _eventStoreClient.ReadStreamAsync(
                    direction,
                    streamId,
                    StreamPosition.Start
                );
            } else if(start < 0 && count > 0) {
                results = _eventStoreClient.ReadStreamAsync(
                    direction,
                    streamId,
                    StreamPosition.End,
                    count
                );
            } else if(start < 0 && count == 0) {
                results = _eventStoreClient.ReadStreamAsync(
                    direction,
                    streamId,
                    StreamPosition.End
                );
            } else {
                results = _eventStoreClient.ReadStreamAsync(
                    direction,
                    streamId,
                    (ulong)start,
                    count
                );
            }

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
