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

        public AggregateServiceTests() {
            List<Event> events = new List<Event>();
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
        }

        [Fact]
        public async void BuildAggregate_NewGameEvent_InvalidId() {
            var aggregate = await aggregateService.BuildAggregate("15", "GameAggregate");
            Assert.Null(aggregate.Id);
        }
    }
}