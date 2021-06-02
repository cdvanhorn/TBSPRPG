using System;
using System.Text.Json;
using TbspRpgLib.InterServiceCommunication;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.Tests.Mocks;
using Xunit;

namespace TbspRpgLib.Tests.InterServiceCommunication
{
    public class UserServiceLinkTests
    {
        #region Setup

        private readonly Guid _testUserToken = Guid.NewGuid();
        private const string _username = "test";
        private const string _password = "test";
        
        private UserServiceLink NewServiceLink()
        {
            return new UserServiceLink(IscUtilities.MockTokenManager(_testUserToken),
                IscUtilities.MockServiceManager());
        }

        #endregion

        #region GetUsers

        [Fact]
        public async void GetUsers_CorrectResponse()
        {
            //arrange
            var serviceLink = NewServiceLink();
            
            //act
            var response = await serviceLink.GetUsers(
                new Credentials()
                {
                    UserId = "userid"
                });
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Response.Content);
            Assert.Equal("user", request.ServiceName);
            Assert.Equal("users", request.EndPoint);
            Assert.Equal(_testUserToken.ToString(), request.Token);
        }

        #endregion

        #region Authenticate

        [Fact]
        public async void Authenticate_CorrectResponse()
        {
            //arrange
            var serviceLink = NewServiceLink();
            var userRequest = new UserRequest()
            {
                Password = _password,
                Username = _username
            };
            
            //act
            var response = await serviceLink.Authenticate(userRequest);
            
            //assert
            var request = JsonSerializer.Deserialize<Request>(response.Response.Content);
            Assert.Equal("user", request.ServiceName);
            Assert.Equal("users/authenticate", request.EndPoint);
            Assert.Null(request.Token);
            Assert.Equal(JsonSerializer.Serialize(userRequest), request.PostData.ToString());
        }

        #endregion
    }
}