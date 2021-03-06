using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Game;
using TbspRpgLib.Events.Game.Content;
using TbspRpgLib.Events.Location;
using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Repositories;
using TbspRpgLib.Services;
using Xunit;

namespace TbspRpgLib.Tests.Aggregate {
    public class AggregateServiceTests {

        private static AggregateService GetAggregateService(IReadOnlyCollection<Event> events) {
            //we need to mock _eventService.GetEventsInStreamAsync for BuildAggregate
            //it needs to return a list of event like objects
            var mockEventService = new Mock<IEventService>();
            mockEventService.Setup(service => 
                service.GetAllEventsInStreamAsync(It.IsAny<string>())
            ).ReturnsAsync(
                (string eventId) =>
                    events.Where(evnt => evnt.StreamId == eventId).ToList()
            );

            var atr = new AggregateTypeRepository();

            return new AggregateService(
                mockEventService.Object, new AggregateTypeService(atr));
        }

        [Fact]
        public async void BuildAggregate_NewGameEventDefaultLanguage_IsValid() {
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

            //act
            var aggregate = await GetAggregateService(events).BuildAggregate("6891aad3-b0fd-4f57-b93b-5ee4fe88917b", "GameAggregate");
            var game = (GameAggregate)aggregate;

            //assert
            Assert.Equal("6891aad3-b0fd-4f57-b93b-5ee4fe88917b", game.Id);
            Assert.Equal("Demo", game.AdventureName);
            Assert.Equal("en", game.Settings.Language);
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
                        AdventureId = "1",
                        Language = "en"
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterEvent(
                    new LocationEnter {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        DestinationLocation = "foo",
                        DestinationRoutes = new List<string> { "One", "Two" },
                        DestinationViaRoute = "beef"
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
            Assert.Equal("foo", game.MapData.DestinationLocation);
            Assert.Equal(2, game.MapData.DestinationRoutes.Count);
            Assert.Equal("One", game.MapData.DestinationRoutes[0]);
            Assert.Equal("beef", game.MapData.DestinationViaRoute);
            Assert.True(game.Checks.Location);
            Assert.Equal("en", game.Settings.Language);
        }
        
        [Fact]
        public async void BuildAggregate_GameAddSourceKey_IsValid() {
            //arrange
            List<Event> events = new List<Event>();
            events.Add(
                new GameNewEvent(
                    new GameNew {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        UserId = "1",
                        AdventureName = "Demo",
                        AdventureId = "1",
                        Language = "en"
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            var sourceKey = Guid.NewGuid();
            events.Add(
                new GameAddSourceKeyEvent(
                    new GameAddSourceKey() {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        SourceKey = sourceKey
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
            Assert.Equal("en", game.Settings.Language);
            Assert.Single(game.SourceKeys);
            Assert.Equal(sourceKey, game.SourceKeys[0]);
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
                        DestinationLocation = "foo",
                        DestinationRoutes = new List<string> { "One", "Two" },
                        DestinationViaRoute = "beef"
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
                        DestinationLocation = "foo",
                        DestinationRoutes = new List<string> { "One", "Two" },
                        DestinationViaRoute = "beef"
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
                        DestinationLocation = "foo",
                        DestinationRoutes = new List<string> { "One", "Two" },
                        DestinationViaRoute = "beef"
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
                        DestinationLocation = "foo",
                        DestinationRoutes = new List<string> { "One", "Two" },
                        DestinationViaRoute = "beef"
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
                        DestinationLocation = "",
                        DestinationRoutes = new List<string>(),
                        DestinationViaRoute = "",
                        CurrentLocation = "foo",
                        CurrentRoutes = new List<string> { "One", "Two" }
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
            Assert.Equal("foo", game.MapData.CurrentLocation);
            Assert.Equal("", game.MapData.DestinationLocation);
            Assert.Equal(2, game.MapData.CurrentRoutes.Count);
            Assert.Empty(game.MapData.DestinationRoutes);
            Assert.Equal("", game.MapData.DestinationViaRoute);
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
                        DestinationLocation = "bar",
                        DestinationRoutes = new List<string> { "One", "Two", "Three" },
                        DestinationViaRoute = "dead"
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
                        DestinationLocation = "",
                        DestinationRoutes = new List<string>(),
                        DestinationViaRoute = "",
                        CurrentLocation = "bar",
                        CurrentRoutes = new List<string> { "One", "Two", "Three" }
                    }
                ) {
                    StreamId = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b"
                }
            );
            events.Add(
                new LocationEnterEvent(
                    new LocationEnter {
                        Id = "6891aad3-b0fd-4f57-b93b-5ee4fe88917b",
                        DestinationLocation = "foo",
                        DestinationRoutes = new List<string> { "Uno", "Dos" },
                        DestinationViaRoute = "beef"
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
                        DestinationLocation = "",
                        DestinationRoutes = new List<string>(),
                        DestinationViaRoute = ""
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
            Assert.Equal("bar", game.MapData.CurrentLocation);
            Assert.Equal(3, game.MapData.CurrentRoutes.Count);
            Assert.Equal("One", game.MapData.CurrentRoutes[0]);
            Assert.Equal("", game.MapData.DestinationLocation);
            Assert.Empty(game.MapData.DestinationRoutes);
            Assert.Equal("", game.MapData.DestinationViaRoute);
            Assert.False(game.Checks.Location);
        }
    }
}