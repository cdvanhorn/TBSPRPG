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

        #region GetContentForGameAfterPosition
        
        [Fact]
        public async void GetContentForGameAfterPosition_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            var testGameId = Guid.NewGuid();
            
            //act
            var response = await serviceLink.GetContentForGameAfterPosition(
                new GameRequest()
                {
                    GameId = testGameId,
                    Position = 40
                }, new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Content);
            Assert.Equal("game", request.ServiceName);
            Assert.Equal($"content/{testGameId}/after/40", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }

        #endregion
        
        #region GetLatestContentForGame

        [Fact]
        public async void GetLatestContentForGame_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            var testGameId = Guid.NewGuid();
            
            //act
            var response = await serviceLink.GetLatestContentForGame(
                new GameRequest()
                {
                    GameId = testGameId
                }, new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Content);
            Assert.Equal("game", request.ServiceName);
            Assert.Equal($"content/{testGameId}/latest", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }

        #endregion

        #region FilterContent

        [Fact]
        public async void FilterContent_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            var testGameId = Guid.NewGuid();
            var filterRequest = new ContentFilterRequest()
            {
                GameId = testGameId,
                Direction = "f",
                Start = 0,
                Count = 1
            };
            
            //act
            var response = await serviceLink.FilterContent(
                filterRequest, new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Content);
            Assert.Equal("game", request.ServiceName);
            Assert.Equal($"content/{testGameId}/filter", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
            Assert.Equal(JsonSerializer.Serialize(filterRequest), request.Parameters.ToString());
        }

        #endregion
    }
}