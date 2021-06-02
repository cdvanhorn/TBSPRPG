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
            Assert.Equal($"content_content/{_testGameId}_{_testUserToken}_null", response.Response.Content);
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
            Assert.Equal($"content_content/latest/{_testGameId}_{_testUserToken}_null", response.Response.Content);
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
            Assert.Equal($"content_content/filter/{_testGameId}_{_testUserToken}_{JsonSerializer.Serialize(filterRequest)}", response.Response.Content);
        }

        #endregion
    }
}