using System;
using System.Dynamic;
using System.Text.Json;
using Moq;
using RestSharp;
using TbspRpgLib.InterServiceCommunication;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.InterServiceCommunication.Utilities;
using Xunit;

namespace TbspRpgLib.Tests.InterServiceCommunication
{
    public class AdventureServiceLinkTests
    {

        #region Setup

        private readonly Guid _userToken = Guid.NewGuid();

        public AdventureServiceLink CreateServiceLink()
        {
            var mockTokenManager = new Mock<ITokenManager>();
            mockTokenManager.Setup(manager => manager.GetTokenForUserId(It.IsAny<string>()))
                .Returns((string userid) => _userToken.ToString());

            var mockServiceManager = new Mock<IServiceManager>();
            mockServiceManager.Setup(sm => sm.MakeGetServiceRequest(It.IsAny<Request>()))
                .ReturnsAsync((Request request) =>
                {
                    return new RestResponse()
                    {
                        Content = $"{request.ServiceName}_{request.EndPoint}_{request.Token}_{request.Parameters}"
                    };
                });
            
            return new AdventureServiceLink(mockTokenManager.Object, mockServiceManager.Object);
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
            Assert.Equal($"adventure_adventures_{_userToken}_", response.Response.Content);
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
            Assert.Equal($"adventure_adventures/demo_{_userToken}_", response.Response.Content);
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
            Assert.Equal($"adventure_adventures/initiallocation/{adventureId}_{_userToken}_", response.Response.Content);
        }

        #endregion
    }
}