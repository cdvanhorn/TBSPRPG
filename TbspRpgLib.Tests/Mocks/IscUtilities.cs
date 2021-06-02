using System;
using Moq;
using RestSharp;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.InterServiceCommunication.Utilities;

namespace TbspRpgLib.Tests.Mocks
{
    public class IscUtilities
    {
        public static IServiceManager MockServiceManager()
        {
            var mockServiceManager = new Mock<IServiceManager>();
            mockServiceManager.Setup(sm => sm.MakeGetServiceRequest(It.IsAny<Request>()))
                .ReturnsAsync((Request request) => new RestResponse()
                {
                    Content = $"{request.ServiceName}_{request.EndPoint}_{request.Token}_{request.Parameters}"
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