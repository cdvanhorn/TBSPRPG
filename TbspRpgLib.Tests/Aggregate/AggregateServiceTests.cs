using System;
using System.Collections.Generic;
using System.Linq;

using Moq;

using Xunit;

using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Content;

namespace TbspRpgLib.Tests.Aggregate {
    public class AggregateServiceTests {
        private AggregateService aggregateService;
        private List<Event> events;

        public AggregateServiceTests() {
            events = new List<Event>();
            events.Add(
                new NewGameEvent(
                    new NewGame {
                        Id = "1",
                        UserId = "1",
                        AdventureName = "Demo",
                        AdventureId = "1"
                    }
                )
            );
            events.Add(
                new EnterLocationEvent(
                    new EnterLocation {
                        Id = "1",
                        Destination = "foo"
                    }
                )
            );
            //we need to mock _eventService.GetEventsInStreamAsync for BuildAggregate
            //it needs to return a list of event like objects
            var mockEventService = new Mock<IEventService>();
            mockEventService.Setup(service => 
                service.GetEventsInStreamAsync(It.IsAny<string>())
            ).ReturnsAsync(
                (string eventid) =>
                    events.Where(evnt => evnt.GetStreamId() == eventid).ToList()
            );

            aggregateService = new AggregateService(mockEventService.Object);
        }

        [Fact]
        public async void BuildAggregate_NewGameEvent_IsValid() {
            //arrange
            //act
            var aggregate = await aggregateService.BuildAggregate("1", "GameAggregate");
            var game = (GameAggregate)aggregate;
            //assert
            Assert.Equal("1", game.Id);
            Assert.Equal("Demo", game.AdventureName);
            Assert.Equal("foo", game.Destination);
        }

        [Fact]
        public async void BuildAggregate_NewGameEvent_InvalidId() {
            //act
            var aggregate = await aggregateService.BuildAggregate("15", "GameAggregate");
            //assert
            Assert.Null(aggregate.Id);
        }

        [Fact]
        public async void BuildAggregate_NewGameEvent_InvalidType() {
            //act, assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => aggregateService.BuildAggregate("15", "Banana"));
            Assert.Equal("invalid aggregate type name Banana", exception.Message);
        }

        [Fact]
        public void HandleEvent_UnProcessed_ExecuteHandler() {
            //arrange
            var evnt = events.FirstOrDefault();
            bool didItRun = false;
            //act
            aggregateService.HandleEvent(evnt,
                (aggregate, eventid, position) => {
                    didItRun = true;
                },
                "foo"
            );
            //assert
            Assert.True(didItRun);
        }

        [Fact]
        public void HandleEvent_Processed_DontExecuteHandler() {
            //arrange
            var evnt = events.FirstOrDefault();
            var processedEvents = new List<Event>();
            processedEvents.Add(evnt);
            processedEvents.Add(
                new EnterLocationEvent(
                    new EnterLocation {
                        Id = "1",
                        Destination = "foo",
                        ProcessedEventId = $"foo_{evnt.EventId}"
                    }
                )
            );
            var originalEvents = events;
            events = processedEvents;
            bool didItRun = false;
            //act
            aggregateService.HandleEvent(evnt,
                (aggregate, eventid, position) => {
                    didItRun = true;
                },
                "foo"
            );
            //assert
            Assert.False(didItRun);
            events = originalEvents;
        }
    }
}