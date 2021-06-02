using System;
using System.Dynamic;
using System.Text.Json;
using Moq;
using RestSharp;
using TbspRpgLib.InterServiceCommunication;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.InterServiceCommunication.Utilities;
using TbspRpgLib.Tests.Mocks;
using Xunit;

namespace TbspRpgLib.Tests.InterServiceCommunication
{
    public class AdventureServiceLinkTests
    {

        #region Setup

        private readonly Guid _userToken = Guid.NewGuid();

        private AdventureServiceLink CreateServiceLink()
        {
            return new AdventureServiceLink(
                IscUtilities.MockTokenManager(_userToken),
                IscUtilities.MockServiceManager());
        }

        #endregion
        

        #region GetAdventures

        [Fact]
        public async void GetAdventures_CorrectRequest()
        {
            //arrange
            var serviceLink = CreateServiceLink();
            
            //act
            var response = await serviceLink.GetAdventures(new Credentials()
            {
                UserId = "userid"
            });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Response.Content);
            Assert.Equal("adventure", request.ServiceName);
            Assert.Equal($"adventures", request.EndPoint);
            Assert.Equal(_userToken.ToString(), request.Token);
        }

        #endregion

        #region GetAdventureByName

        [Fact]
        public async void GetAdventureByName_CorrectRequest()
        {
            //arrange
            var serviceLink = CreateServiceLink();
            
            //act
            var response = await serviceLink.GetAdventureByName(new AdventureRequest()
            {
                Name = "demo"
            }, new Credentials()
            {
                UserId = "userid"
            });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Response.Content);
            Assert.Equal("adventure", request.ServiceName);
            Assert.Equal($"adventures/demo", request.EndPoint);
            Assert.Equal(_userToken.ToString(), request.Token);
        }

        #endregion
        
        #region GetInitialLocation

        [Fact]
        public async void GetInitialLocation_CorrectRequest()
        {
            //arrange
            var serviceLink = CreateServiceLink();
            var adventureId = Guid.NewGuid();
            
            //act
            var response = await serviceLink.GetInitialLocation(new AdventureRequest()
            {
                Id = adventureId,
                Name = "demo"
            }, new Credentials()
            {
                UserId = "userid"
            });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Response.Content);
            Assert.Equal("adventure", request.ServiceName);
            Assert.Equal($"adventures/initiallocation/{adventureId}", request.EndPoint);
            Assert.Equal(_userToken.ToString(), request.Token);
        }

        #endregion
    }
}