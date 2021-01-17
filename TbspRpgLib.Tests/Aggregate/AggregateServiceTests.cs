using System;
using System.Collections.Generic;
using System.Linq;

using Moq;

using Xunit;

using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Content;
using TbspRpgLib.Tests.Mocks;
using TbspRpgLib.Services;

namespace TbspRpgLib.Tests.Aggregate {
    public class AggregateServiceTests {
        private AggregateService aggregateService;
        private List<Event> events;
        private ServiceTrackingService _stService;

        public AggregateServiceTests() {
            events = new List<Event>();
            events.Add(
                new NewGameEvent(
                    new NewGame {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        UserId = "1",
                        AdventureName = "Demo",
                        AdventureId = "1"
                    }
                )
            );
            events.Add(
                new EnterLocationEvent(
                    new EnterLocation {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
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

            _stService = ServiceTrackingServiceMock.MockServiceTrackingService();
            aggregateService = new AggregateService(
                mockEventService.Object,
                _stService);
        }

        [Fact]
        public async void BuildAggregate_NewGameEvent_IsValid() {
            //arrange
            //act
            var aggregate = await aggregateService.BuildAggregate("6891aad3-b0fd-4f57-b93b-5ee4fe88917b", "GameAggregate");
            var game = (GameAggregate)aggregate;
            //assert
            Assert.Equal("6891aad3-b0fd-4f57-b93b-5ee4fe88917b", game.Id);
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
                new Guid("f649b3f1-b69d-43d0-adbd-1e188f2cdae9")
            );
            //assert
            Assert.True(didItRun);
        }

        [Fact]
        public async void HandleEvent_Processed_DontExecuteHandler() {
            //arrange
            var evnt = events.FirstOrDefault();
            //we need to add this event to the database as processed
            await _stService.EventProcessed(
                new Guid("f649b3f1-b69d-43d0-adbd-1e188f2cdae9"),
                evnt.EventId);
            bool didItRun = false;
            
            //act
            aggregateService.HandleEvent(evnt,
                (aggregate, eventid, position) => {
                    didItRun = true;
                },
                new Guid("f649b3f1-b69d-43d0-adbd-1e188f2cdae9")
            );
            //assert
            Assert.False(didItRun);
        }
    }
}