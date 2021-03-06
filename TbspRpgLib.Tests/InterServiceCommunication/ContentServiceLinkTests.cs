using System;
using System.Text.Json;
using TbspRpgLib.InterServiceCommunication;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.Tests.Mocks;
using Xunit;

namespace TbspRpgLib.Tests.InterServiceCommunication
{
    public class ContentServiceLinkTests
    {
        #region Setup

        private readonly Guid _testUserToken = Guid.NewGuid();
        private readonly Guid _testGameId = Guid.NewGuid();

        private ContentServiceLink NewServiceLink()
        {
            return new ContentServiceLink(IscUtilities.MockTokenManager(_testUserToken),
                IscUtilities.MockServiceManager());
        }

        #endregion

        #region GetContentForGame

        [Fact]
        public async void GetContentForGame_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            
            //act
            var response = await serviceLink.GetContentForGame(
                new ContentRequest()
                {
                    GameId = _testGameId
                }, new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Content);
            Assert.Equal("content", request.ServiceName);
            Assert.Equal($"content/{_testGameId}", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }

        #endregion

        #region GetLatestContentForGame

        [Fact]
        public async void GetLatestContentForGame_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            
            //act
            var response = await serviceLink.GetLatestContentForGame(
                new ContentRequest()
                {
                    GameId = _testGameId
                }, new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Content);
            Assert.Equal("content", request.ServiceName);
            Assert.Equal($"content/latest/{_testGameId}", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }

        #endregion

        #region FilterContent

        [Fact]
        public async void FilterContent_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            var filterRequest = new ContentFilterRequest()
            {
                GameId = _testGameId,
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
            Assert.Equal("content", request.ServiceName);
            Assert.Equal($"content/filter/{_testGameId}", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
            Assert.Equal(JsonSerializer.Serialize(filterRequest), request.Parameters.ToString());
        }

        #endregion

        #region GetSourceContent

        [Fact]
        public async void GetSourceContent_WithLanguage_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            var sourceKey = Guid.NewGuid();
            
            //act
            var response = await serviceLink.GetSourceContent(
                new ContentRequest()
                {
                    GameId = _testGameId,
                    Language = "en",
                    SourceKey = sourceKey
                }, new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Content);
            Assert.Equal("content", request.ServiceName);
            Assert.Equal($"content/source/en/{sourceKey}", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }
        
        [Fact]
        public async void GetSourceContent_WithGame_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            var sourceKey = Guid.NewGuid();
            
            //act
            var response = await serviceLink.GetSourceContent(
                new ContentRequest()
                {
                    GameId = _testGameId,
                    SourceKey = sourceKey
                }, new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Content);
            Assert.Equal("content", request.ServiceName);
            Assert.Equal($"content/source/{_testGameId}/{sourceKey}", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }

        #endregion
        
        #region GetContentForGameAfterPosition

        [Fact]
        public async void GetContentForGameAfterPosition_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            
            //act
            var response = await serviceLink.GetContentForGameAfterPosition(
                new ContentRequest()
                {
                    GameId = _testGameId,
                    Position = 40
                }, new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Content);
            Assert.Equal("content", request.ServiceName);
            Assert.Equal($"content/{_testGameId}/after/40", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }

        #endregion
    }
}