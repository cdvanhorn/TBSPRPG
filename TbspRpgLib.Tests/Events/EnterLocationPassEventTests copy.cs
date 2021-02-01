using System;
using System.Text.Json;

using Xunit;

using TbspRpgLib.Events;
using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Events.Location;

namespace TbspRpgLib.Tests.Events {
    public class EnterLocationPassEventTests {
        private LocationEnterPassEvent CreateEnterLocationPassEvent() {
            return new LocationEnterPassEvent(
                new LocationEnterPass {
                    Id = "2",
                    CurrentLocation = "foo",
                    Destination = ""
                }
            );
        }

        [Fact]
        public void EnterLocationPassEvent_Creation_Valid() {
            //arrange, act
            var evnt = CreateEnterLocationPassEvent();
            //assert
            Assert.IsType<Guid>(evnt.EventId);
            Assert.Equal(Event.LOCATION_ENTER_PASS_EVENT_TYPE, evnt.Type);
        }

        [Fact]
        public void EnterLocationPassEvent_GetDataJson_Valid() {
            //arrange
            var evnt = CreateEnterLocationPassEvent();
            //act
            var jsonString = evnt.GetDataJson();
            LocationEnterPass content = JsonSerializer.Deserialize<LocationEnterPass>(jsonString);

            //assert
            Assert.IsType<LocationEnterPass>(content);
            Assert.Equal("2", content.Id);
            Assert.Equal("foo", content.CurrentLocation);
            Assert.Equal("", content.Destination);
        }

        [Fact]
        public void EnterLocationPassEvent_GetStreamId_Valid() {
            //arrange
            var evnt = CreateEnterLocationPassEvent();
            //act
            var streamid = evnt.GetStreamId();
            //assert
            Assert.Equal("2", streamid);
        }
    }   
}