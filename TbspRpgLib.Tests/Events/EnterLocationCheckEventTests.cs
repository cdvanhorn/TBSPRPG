using System;
using System.Text.Json;

using Xunit;

using TbspRpgLib.Events;
using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Events.Location;

namespace TbspRpgLib.Tests.Events {
    public class EnterLocationCheckEventTests {
        private LocationEnterCheckEvent CreateEnterLocationCheckEvent() {
            return new LocationEnterCheckEvent(
                new LocationEnterCheck {
                    Id = "2",
                    Result = false
                }
            );
        }

        [Fact]
        public void EnterLocationCheckEvent_Creation_Valid() {
            //arrange, act
            var evnt = CreateEnterLocationCheckEvent();
            //assert
            Assert.IsType<Guid>(evnt.EventId);
            Assert.Equal(Event.LOCATION_ENTER_CHECK_EVENT_TYPE, evnt.Type);
        }

        [Fact]
        public void EnterLocationEvent_GetDataJson_Valid() {
            //arrange
            var evnt = CreateEnterLocationCheckEvent();
            //act
            var jsonString = evnt.GetDataJson();
            LocationEnterCheck content = JsonSerializer.Deserialize<LocationEnterCheck>(jsonString);

            //assert
            Assert.IsType<LocationEnterCheck>(content);
            Assert.Equal("2", content.Id);
            Assert.False(content.Result);
        }

        // [Fact]
        // public void EnterLocationEvent_GetStreamId_Valid() {
        //     //arrange
        //     var evnt = CreateEnterLocationCheckEvent();
        //     //act
        //     var streamid = evnt.GetStreamId();
        //     //assert
        //     Assert.Equal("2", streamid);
        // }
    }   
}