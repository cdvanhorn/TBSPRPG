using System;
using System.Text.Json;
using TbspRpgLib.Events;
using TbspRpgLib.Events.Game;
using TbspRpgLib.Events.Game.Content;
using Xunit;

namespace TbspRpgLib.Tests.Events
{
    public class GameAddSourceKeyEventTests
    {
        private readonly Guid _sourceKey = Guid.NewGuid();
        private GameAddSourceKeyEvent CreateAddSourceKeyEvent() {
            return new GameAddSourceKeyEvent(
                new GameAddSourceKey() {
                    Id = "1",
                    SourceKey = _sourceKey
                }
            );
        }

        [Fact]
        public void GameAddSourceKeyEvent_Creation_Valid() {
            //arrange, act
            var evnt = CreateAddSourceKeyEvent();
            //assert
            Assert.IsType<Guid>(evnt.EventId);
            Assert.Equal(Event.GAME_ADD_SOURCE_KEY_EVENT_TYPE, evnt.Type);
        }

        [Fact]
        public void GameAddSourceKeyEvent_GetDataJson_Valid() {
            //arrange
            var gameAddSourceKeyEvent = CreateAddSourceKeyEvent();
            //act
            var jsonString = gameAddSourceKeyEvent.GetDataJson();
            var content = JsonSerializer.Deserialize<GameAddSourceKey>(jsonString);

            //assert
            Assert.IsType<GameAddSourceKey>(content);
            Assert.Equal("1", content.Id);
            Assert.Equal(_sourceKey, content.SourceKey);
        }
    }
}