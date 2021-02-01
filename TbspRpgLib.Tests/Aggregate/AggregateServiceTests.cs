using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Moq;

using Xunit;

using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Events.Location;
using TbspRpgLib.Events.Game.Content;
using TbspRpgLib.Events.Game;
using TbspRpgLib.Tests.Mocks;
using TbspRpgLib.Services;

namespace TbspRpgLib.Tests.Aggregate {
    public class AggregateServiceTests {

        public AggregateService GetAggregateService(List<Event> events) {
            //we need to mock _eventService.GetEventsInStreamAsync for BuildAggregate
            //it needs to return a list of event like objects
            var mockEventService = new Mock<IEventService>();
            mockEventService.Setup(service => 
                service.GetEventsInStreamAsync(It.IsAny<string>())
            ).ReturnsAsync(
                (string eventid) =>
                    events.Where(evnt => evnt.GetStreamId() == eventid).ToList()
            );

            return new AggregateService(
                mockEventService.Object);
        }


        [Fact]
        public async void BuildAggregate_NewGameEvent_IsValid() {
            //arrange
            List<Event> events = new List<Event>();
            events.Add(
                new GameNewEvent(
                    new GameNew {
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

            //act
            var aggregate = await GetAggregateService(events).BuildAggregate("6891aad3-b0fd-4f57-b93b-5ee4fe88917b", "GameAggregate");
            var game = (GameAggregate)aggregate;

            //assert
            Assert.Equal("6891aad3-b0fd-4f57-b93b-5ee4fe88917b", game.Id);
            Assert.Equal("Demo", game.AdventureName);
            Assert.Equal("foo", game.Destination);
            Assert.True(game.Checks.Location);
        }

        [Fact]
        public async void BuildAggregate_NewGameEvent_InvalidId() {
            //arrange
            //arrange
            List<Event> events = new List<Event>();
            events.Add(
                new GameNewEvent(
                    new GameNew {
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

            //act
            var aggregate = await GetAggregateService(events).BuildAggregate("15", "GameAggregate");
            //assert
            Assert.Null(aggregate.Id);
        }

        [Fact]
        public async void BuildAggregate_NewGameEvent_InvalidType() {
            //arrange
            List<Event> events = new List<Event>();
            events.Add(
                new GameNewEvent(
                    new GameNew {
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

            //act, assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => GetAggregateService(events).BuildAggregate("15", "Banana"));
            Assert.Equal("invalid aggregate type name Banana", exception.Message);
        }

        [Fact]
        public async void HandleEvent_UnProcessed_ExecuteHandler() {
            //arrange
            List<Event> events = new List<Event>();
            events.Add(
                new GameNewEvent(
                    new GameNew {
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

            var evnt = events.FirstOrDefault();
            bool didItRun = false;
            //act
            await GetAggregateService(events).HandleEvent(evnt,
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