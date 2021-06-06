using System;
using System.Collections.Generic;
using System.Text.Json;

using Xunit;

using TbspRpgLib.Events;
using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Events.Location;

namespace TbspRpgLib.Tests.Events {
    public class EnterLocationFailEventTests {
        private static LocationEnterFailEvent CreateEnterLocationFailEvent() {
            return new LocationEnterFailEvent(
                new LocationEnterFail {
                    Id = "2",
                    DestinationLocation = "",
                    DestinationRoutes = new List<string>()
                }
            );
        }

        [Fact]
        public void EnterLocationFailEvent_Creation_Valid() {
            //arrange, act
            var evnt = CreateEnterLocationFailEvent();
            //assert
            Assert.IsType<Guid>(evnt.EventId);
            Assert.Equal(Event.LOCATION_ENTER_FAIL_EVENT_TYPE, evnt.Type);
        }

        [Fact]
        public void EnterLocationFailEvent_GetDataJson_Valid() {
            //arrange
            var evnt = CreateEnterLocationFailEvent();
            //act
            var jsonString = evnt.GetDataJson();
            LocationEnterFail content = JsonSerializer.Deserialize<LocationEnterFail>(jsonString);

            //assert
            Assert.IsType<LocationEnterFail>(content);
            Assert.Equal("2", content.Id);
            Assert.Equal("", content.DestinationLocation);
            Assert.Empty(content.DestinationRoutes);
        }

        // [Fact]
        // public void EnterLocationFailEvent_GetStreamId_Valid() {
        //     //arrange
        //     var evnt = CreateEnterLocationFailEvent();
        //     //act
        //     var streamid = evnt.GetStreamId();
        //     //assert
        //     Assert.Equal("2", streamid);
        // }
    }   
}