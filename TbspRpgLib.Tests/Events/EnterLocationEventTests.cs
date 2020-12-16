using System;
using System.Text.Json;

using Xunit;

using TbspRpgLib.Events;
using TbspRpgLib.Events.Content;

namespace TbspRpgLib.Tests.Events {
    public class EnterLocationEventTests {
        private EnterLocationEvent CreateEnterLocationEvent() {
            return new EnterLocationEvent(
                new EnterLocation {
                    Id = "2",
                    Destination = "foo",
                    ProcessedEventId = $"bar_{Guid.NewGuid().ToString()}"
                }
            );
        }

        [Fact]
        public void EnterLocationEvent_Creation_Valid() {
            //arrange, act
            var evnt = CreateEnterLocationEvent();
            //assert
            Assert.IsType<Guid>(evnt.EventId);
            Assert.Equal(Event.ENTER_LOCATION_EVENT_TYPE, evnt.Type);
        }

        [Fact]
        public void EnterLocationEvent_GetDataJson_Valid() {
            //arrange
            var evnt = CreateEnterLocationEvent();
            //act
            var jsonString = evnt.GetDataJson();
            EnterLocation content = JsonSerializer.Deserialize<EnterLocation>(jsonString);

            //assert
            Assert.IsType<EnterLocation>(content);
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