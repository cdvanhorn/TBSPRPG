using System;
using System.Text.Json;

using Xunit;

using TbspRpgLib.Events;
using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Events.Location;

namespace TbspRpgLib.Tests.Events {
    public class EnterLocationEventTests {
        private LocationEnterEvent CreateEnterLocationEvent() {
            return new LocationEnterEvent(
                new LocationEnter {
                    Id = "2",
                    Destination = "foo"
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
            Assert.Equal("foo", content.Destination);
        }

        [Fact]
        public void EnterLocationEvent_GetStreamId_Valid() {
            //arrange
            var evnt = CreateEnterLocationEvent();
            //act
            var streamid = evnt.GetStreamId();
            //assert
            Assert.Equal("2", streamid);
        }
    }   
}