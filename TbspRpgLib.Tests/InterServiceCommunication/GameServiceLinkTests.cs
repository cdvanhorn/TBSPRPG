using System;
using System.Text.Json;
using TbspRpgLib.InterServiceCommunication;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.Tests.Mocks;
using Xunit;

namespace TbspRpgLib.Tests.InterServiceCommunication
{
    public class GameServiceLinkTests
    {
        #region Setup

        private readonly Guid _testUserToken = Guid.NewGuid();
        private readonly Guid _testAdventureId = Guid.NewGuid();
        
        private GameServiceLink NewServiceLink()
        {
            return new GameServiceLink(IscUtilities.MockTokenManager(_testUserToken),
                IscUtilities.MockServiceManager());
        }

        #endregion

        #region GetGames

        [Fact]
        public async void GetGames_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            
            //act
            var response = await serviceLink.GetGames(
                new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Content);
            Assert.Equal("game", request.ServiceName);
            Assert.Equal("games", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }

        #endregion

        #region StartGame

        [Fact]
        public async void Start_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            
            //act
            var response = await serviceLink.StartGame(
                new GameRequest()
                {
                  AdventureId  = _testAdventureId
                },
                new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Content);
            Assert.Equal("game", request.ServiceName);
            Assert.Equal($"games/start/{_testAdventureId}", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }

        #endregion

        #region GetGameForAdventure
        
        [Fact]
        public async void GetGameForAdventure_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            
            //act
            var response = await serviceLink.GetGameForAdventure(
                new GameRequest()
                {
                    AdventureId = _testAdventureId
                }, 
                new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Content);
            Assert.Equal("game", request.ServiceName);
            Assert.Equal($"games/{_testAdventureId}", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }

        #endregion
    }
}