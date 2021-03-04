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
using TbspRpgLib.Events.Content;
using TbspRpgLib.Tests.Mocks;
using TbspRpgLib.Services;
using TbspRpgLib.Repositories;

namespace TbspRpgLib.Tests.Aggregate {
    public class AggregateServiceTests {

        public AggregateService GetAggregateService(List<Event> events) {
            //we need to mock _eventService.GetEventsInStreamAsync for BuildAggregate
            //it needs to return a list of event like objects
            var mockEventService = new Mock<IEventService>();
            mockEventService.Setup(service => 
                service.GetAllEventsInStreamAsync(It.IsAny<string>())
            ).ReturnsAsync(
                (string eventid) =>
                    events.Where(evnt => evnt.StreamId == eventid).ToList()
            );

            var atr = new AggregateTypeRepository();

            return new AggregateService(
                mockEventService.Object, new AggregateTypeService(atr));
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
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterEvent(
                    new LocationEnter {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Destination = "foo"
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterCheckEvent(
                    new LocationEnterCheck {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Result = true
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
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
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterEvent(
                    new LocationEnter {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Destination = "foo"
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
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
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterEvent(
                    new LocationEnter {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Destination = "foo"
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
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
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterEvent(
                    new LocationEnter {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Destination = "foo"
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
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

         [Fact]
        public async void BuildAggregate_LocationEnterPassEvent_InvalidId() {
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
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterEvent(
                    new LocationEnter {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Destination = "foo"
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterCheckEvent(
                    new LocationEnterCheck {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Result = true
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterPassEvent(
                    new LocationEnterPass {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Destination = "",
                        CurrentLocation = "foo"
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );

            //act
            var aggregate = await GetAggregateService(events).BuildAggregate("6891aad3-b0fd-4f57-b93b-5ee4fe88917b", "GameAggregate");
            var game = (GameAggregate)aggregate;

            //assert
            Assert.Equal("6891aad3-b0fd-4f57-b93b-5ee4fe88917b", game.Id);
            Assert.Equal("Demo", game.AdventureName);
            Assert.Equal("foo", game.CurrentLocation);
            Assert.Equal("", game.Destination);
            Assert.False(game.Checks.Location);
        }

        [Fact]
        public async void BuildAggregate_LocationEnterFailEvent_Valid() {
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
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterEvent(
                    new LocationEnter {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Destination = "bar"
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterCheckEvent(
                    new LocationEnterCheck {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Result = true
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterPassEvent(
                    new LocationEnterPass {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Destination = "",
                        CurrentLocation = "bar"
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterEvent(
                    new LocationEnter {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Destination = "foo"
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterCheckEvent(
                    new LocationEnterCheck {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Result = false
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterFailEvent(
                    new LocationEnterFail {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Destination = ""
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );

            //act
            var aggregate = await GetAggregateService(events).BuildAggregate("6891aad3-b0fd-4f57-b93b-5ee4fe88917b", "GameAggregate");
            var game = (GameAggregate)aggregate;

            //assert
            Assert.Equal("6891aad3-b0fd-4f57-b93b-5ee4fe88917b", game.Id);
            Assert.Equal("Demo", game.AdventureName);
            Assert.Equal("bar", game.CurrentLocation);
            Assert.Equal("", game.Destination);
            Assert.False(game.Checks.Location);
        }

        [Fact]
        public async void HandleContentEvent_UnProcessed_ExecuteHandler() {
            //arrange
            List<Event> events = new List<Event>();
            events.Add(
                new ContentEvent(
                    new ContentContent {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Text = "Event1"
                    }
                ) {
                    StreamId = "content_6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new ContentEvent(
                    new ContentContent {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        Text = "Event2"
                    }
                ) {
                    StreamId = "content_6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );

            var evnt = events.FirstOrDefault();
            bool didItRun = false;
            TbspRpgLib.Aggregates.Aggregate agg = null;
            //act
            await GetAggregateService(events).HandleEvent(evnt,
                (aggregate, eventid) => {
                    agg = aggregate;
                    didItRun = true;
                    return Task.CompletedTask;
                }
            );

            //assert
            var cagg = (ContentAggregate)agg;
            Assert.True(didItRun);
            Assert.IsType<ContentAggregate>(agg);
            Assert.IsType<List<string>>(cagg.Text);
            Assert.Equal("Event1", cagg.Text[0]);
        }
    }
}