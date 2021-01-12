using System;
using System.Text.Json;

using Xunit;

using TbspRpgLib.Events;
using TbspRpgLib.Events.Content;

namespace TbspRpgLib.Tests.Events {
    public class NewGameEventTests {
        private NewGameEvent CreateNewGameEvent() {
            return new NewGameEvent(
                new NewGame {
                    Id = "1",
                    UserId = "1",
                    AdventureId = "1",
                    AdventureName = "Demo"
                }
            );
        }

        [Fact]
        public void NewGameEvent_Creation_Valid() {
            //arrange, act
            var newGame = CreateNewGameEvent();
            //assert
            Assert.IsType<Guid>(newGame.EventId);
            Assert.Equal(Event.NEW_GAME_EVENT_TYPE, newGame.Type);
        }

        [Fact]
        public void NewGameEvent_GetDataJson_Valid() {
            //arrange
            var ngame = CreateNewGameEvent();
            //act
            var jsonString = ngame.GetDataJson();
            NewGame content = JsonSerializer.Deserialize<NewGame>(jsonString);

            //assert
            Assert.IsType<NewGame>(content);
            Assert.Equal("1", content.Id);
            Assert.Equal("1", content.UserId);
            Assert.Equal("1", content.AdventureId);
            Assert.Equal("Demo", content.AdventureName);
        }

        [Fact]
        public void NewGameEvent_GetStreamId_Valid() {
            //arrange
            var ngame = CreateNewGameEvent();
            //act
            var streamid = ngame.GetStreamId();
            //assert
            Assert.Equal("1", streamid);
        }
    }   
}