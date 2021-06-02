using System;
using System.Text.Json;
using Moq;
using RestSharp;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.InterServiceCommunication.Utilities;

namespace TbspRpgLib.Tests.Mocks
{
    public static class IscUtilities
    {
        public static IServiceManager MockServiceManager()
        {
            var mockServiceManager = new Mock<IServiceManager>();
            mockServiceManager.Setup(sm => sm.MakeGetServiceRequest(It.IsAny<Request>()))
                .ReturnsAsync((Request request) => new RestResponse()
                {
                    Content = JsonSerializer.Serialize(request)
                });
            mockServiceManager.Setup(sm => sm.MakePostServiceRequestNoAuth(It.IsAny<Request>()))
                .ReturnsAsync((Request request) => new RestResponse()
                {
                    Content = JsonSerializer.Serialize(request)
                });
            return mockServiceManager.Object;
        }

        public static ITokenManager MockTokenManager(Guid userToken)
        {
            var mockTokenManager = new Mock<ITokenManager>();
            mockTokenManager.Setup(manager => manager.GetTokenForUserId(It.IsAny<string>()))
                .Returns((string userid) => userToken.ToString());
            return mockTokenManager.Object;
        }
    }
}