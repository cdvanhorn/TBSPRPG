using System;
using System.Text.Json;

using Xunit;

using TbspRpgLib.Events;
using TbspRpgLib.Events.Game.Content;
using TbspRpgLib.Events.Game;

namespace TbspRpgLib.Tests.Events {
    public class NewGameEventTests {
        private GameNewEvent CreateNewGameEvent() {
            return new GameNewEvent(
                new GameNew {
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
            Assert.Equal(Event.GAME_NEW_EVENT_TYPE, newGame.Type);
        }

        [Fact]
        public void NewGameEvent_GetDataJson_Valid() {
            //arrange
            var ngame = CreateNewGameEvent();
            //act
            var jsonString = ngame.GetDataJson();
            GameNew content = JsonSerializer.Deserialize<GameNew>(jsonString);

            //assert
            Assert.IsType<GameNew>(content);
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