using System;
using System.Text.Json;

using Xunit;

using TbspRpgLib.Events;
using TbspRpgLib.Events.Content;

namespace TbspRpgLib.Tests.Events {
    public class ContentEventTests {
        private ContentEvent CreateContentEvent() {
            return new ContentEvent(
                new ContentContent {
                    Id = "2",
                    Text = "ContentTwo"
                }
            );
        }

        [Fact]
        public void ContentEvent_Creation_Valid() {
            //arrange, act
            var evnt = CreateContentEvent();
            //assert
            Assert.IsType<Guid>(evnt.EventId);
            Assert.Equal(Event.CONTENT_EVENT_TYPE, evnt.Type);
        }

        [Fact]
        public void EnterLocationEvent_GetDataJson_Valid() {
            //arrange
            var evnt = CreateContentEvent();
            //act
            var jsonString = evnt.GetDataJson();
            ContentContent content = JsonSerializer.Deserialize<ContentContent>(jsonString);

            //assert
            Assert.IsType<ContentContent>(content);
            Assert.Equal("2", content.Id);
            Assert.Equal("ContentTwo", content.Text);
        }

        [Fact]
        public void EnterLocationEvent_GetStreamId_Valid() {
            //arrange
            var evnt = CreateContentEvent();
            //act
            var streamid = evnt.GetStreamId();
            //assert
            Assert.Equal("content_2", streamid);
        }
    }   
}