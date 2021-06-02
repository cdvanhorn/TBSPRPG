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
            Assert.Equal($"adventure_adventures_{_userToken}_null", response.Response.Content);
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
            Assert.Equal($"adventure_adventures/demo_{_userToken}_null", response.Response.Content);
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
            Assert.Equal($"adventure_adventures/initiallocation/{adventureId}_{_userToken}_null", response.Response.Content);
        }

        #endregion
    }
}