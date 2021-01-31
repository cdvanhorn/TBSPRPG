using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Moq;

using Xunit;

using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Content;
using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Events.Location;
using TbspRpgLib.Tests.Mocks;
using TbspRpgLib.Services;

namespace TbspRpgLib.Tests.Aggregate {
    public class AggregateServiceTests {
        private AggregateService aggregateService;
        private List<Event> events;

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
                new LocationEnterEvent(
                    new LocationEnter {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Destination = "foo"
                    }
                )
            );
            events.Add(
                new LocationEnterCheckEvent(
                    new LocationEnterCheck {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Result = true
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

            aggregateService = new AggregateService(
                mockEventService.Object);
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
            Assert.True(game.Checks.Location);
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
        public async void HandleEvent_UnProcessed_ExecuteHandler() {
            //arrange
            var evnt = events.FirstOrDefault();
            bool didItRun = false;
            //act
            await aggregateService.HandleEvent(evnt,
                (aggregate, eventid) => {
                    didItRun = true;
                    return Task.CompletedTask;
                }
            );
            //assert
            Assert.True(didItRun);
        }
    }
}