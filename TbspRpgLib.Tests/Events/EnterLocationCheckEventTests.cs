using System;
using System.Text.Json;

using Xunit;

using TbspRpgLib.Events;
using TbspRpgLib.Events.Content;

namespace TbspRpgLib.Tests.Events {
    public class EnterLocationCheckEventTests {
        private EnterLocationCheckEvent CreateEnterLocationCheckEvent() {
            return new EnterLocationCheckEvent(
                new EnterLocationCheck {
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
            Assert.Equal(Event.ENTER_LOCATION_CHECK_EVENT_TYPE, evnt.Type);
        }

        [Fact]
        public void EnterLocationEvent_GetDataJson_Valid() {
            //arrange
            var evnt = CreateEnterLocationCheckEvent();
            //act
            var jsonString = evnt.GetDataJson();
            EnterLocationCheck content = JsonSerializer.Deserialize<EnterLocationCheck>(jsonString);

            //assert
            Assert.IsType<EnterLocationCheck>(content);
            Assert.Equal("2", content.Id);
            Assert.False(content.Result);
        }

        [Fact]
        public void EnterLocationEvent_GetStreamId_Valid() {
            //arrange
            var evnt = CreateEnterLocationCheckEvent();
            //act
            var streamid = evnt.GetStreamId();
            //assert
            Assert.Equal("2", streamid);
        }
    }   
}