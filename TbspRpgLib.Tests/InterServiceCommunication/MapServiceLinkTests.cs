using System;
using System.Text.Json;
using TbspRpgLib.InterServiceCommunication;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.Tests.Mocks;
using Xunit;

namespace TbspRpgLib.Tests.InterServiceCommunication
{
    public class MapServiceLinkTests
    {
        #region Setup

        private readonly Guid _testUserToken = Guid.NewGuid();
        private readonly Guid _testGameId = Guid.NewGuid();
        
        private MapServiceLink NewServiceLink()
        {
            return new MapServiceLink(IscUtilities.MockTokenManager(_testUserToken),
                IscUtilities.MockServiceManager());
        }

        #endregion

        #region GetLocations

        [Fact]
        public async void GetLocations_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            
            //act
            var response = await serviceLink.GetLocations(
                new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Response.Content);
            Assert.Equal("map", request.ServiceName);
            Assert.Equal("locations", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }

        #endregion

        #region GetLocationByGameId

        [Fact]
        public async void GetLocationByGameId_CorrectRequest()
        {
            //arrange
            var serviceLink = NewServiceLink();
            
            //act
            var response = await serviceLink.GetLocationByGameId(
                new MapRequest()
                {
                    GameId = _testGameId
                },
                new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Response.Content);
            Assert.Equal("map", request.ServiceName);
            Assert.Equal($"locations/{_testGameId}", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }

        #endregion
    }
}