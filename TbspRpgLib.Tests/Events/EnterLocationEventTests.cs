using System;
using System.Collections.Generic;
using System.Text.Json;

using Xunit;

using TbspRpgLib.Events;
using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Events.Location;

namespace TbspRpgLib.Tests.Events {
    public class EnterLocationEventTests {
        private static LocationEnterEvent CreateEnterLocationEvent() {
            return new LocationEnterEvent(
                new LocationEnter {
                    Id = "2",
                    DestinationLocation = "foo",
                    DestinationRoutes = new List<string> { "One", "Two", "Three" }
                }
            );
        }

        [Fact]
        public void EnterLocationEvent_Creation_Valid() {
            //arrange, act
            var evnt = CreateEnterLocationEvent();
            //assert
            Assert.IsType<Guid>(evnt.EventId);
            Assert.Equal(Event.LOCATION_ENTER_EVENT_TYPE, evnt.Type);
        }

        [Fact]
        public void EnterLocationEvent_GetDataJson_Valid() {
            //arrange
            var evnt = CreateEnterLocationEvent();
            //act
            var jsonString = evnt.GetDataJson();
            LocationEnter content = JsonSerializer.Deserialize<LocationEnter>(jsonString);

            //assert
            Assert.IsType<LocationEnter>(content);
            Assert.Equal("2", content.Id);
            Assert.Equal("foo", content.DestinationLocation);
            Assert.Equal(3, content.DestinationRoutes.Count);
            Assert.Equal("One", content.DestinationRoutes[0]);
        }

        // [Fact]
        // public void EnterLocationEvent_GetStreamId_Valid() {
        //     //arrange
        //     var evnt = CreateEnterLocationEvent();
        //     //act
        //     var streamid = evnt.GetStreamId();
        //     //assert
        //     Assert.Equal("2", streamid);
        // }
    }   
}