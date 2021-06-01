using System.Buffers;
using System.Collections.Generic;
using System.Dynamic;
using RestSharp;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.InterServiceCommunication.Utilities;
using TbspRpgLib.Repositories;
using TbspRpgLib.Services;
using Xunit;

namespace TbspRpgLib.Tests.InterServiceCommunication.Utilities
{
    class HttpBinResponseHeaders
    {
        public string Accept { get; set; }
        public string AcceptEncoding { get; set; }
        public string Authorization { get; set; }
        public string Host { get; set; }
        public string UserAgent { get; set; }
        public string XAmznTraceId { get; set; }
    }
    
    class HttpBinResponse
    {
        public Dictionary<string, string> Args { get; set; }
        public HttpBinResponseHeaders Headers { get; set; }
        public Request Json { get; set; }
        public string Origin { get; set; }
        public string Url { get; set; }
    }
    
    public class ServiceManagerTests
    {
        #region Setup

        private readonly RestClient _testClient;
        private const string _testServiceName = "TestService";
        private readonly IServiceService _serviceService;

        public ServiceManagerTests()
        {
            _testClient = new RestClient("https://httpbin.org");
            _serviceService = new ServiceService(new ServiceRepository());
        }

        private void Seed(IServiceManager manager)
        {
            manager.AddClientForServiceName(_testServiceName, _testClient);
        }

        private ServiceManager CreateServiceManager()
        {
            return new ServiceManager(_serviceService);
        }

        #endregion
        
        #region GetClientForServiceName

        [Fact]
        public void GetClientForServiceName_DoesntExist_ClientCreated()
        {
            //arrange
            var manager = CreateServiceManager();
            Seed(manager);
            
            //act
            var client = manager.GetClientForServiceName("adventure");
            
            //assert
            Assert.Equal(_serviceService.GetUrlForService("adventure"), client.BaseUrl.ToString());
        }

        [Fact]
        public void GetClientForServiceName_Exists_ReturnsExisting()
        {
            //arrange
            var manager = CreateServiceManager();
            Seed(manager);
            
            //act
            var client = manager.GetClientForServiceName(_testServiceName);
            
            //assert
            Assert.True(ReferenceEquals(_testClient, client));
        }

        [Fact]
        public void GetClientForServiceName_NoCache_ClientCreated()
        {
            //arrange
            var manager = CreateServiceManager();
            Seed(manager);
            manager.CacheService = false;
            
            //act
            var client = manager.GetClientForServiceName("adventure");
            var client2 = manager.GetClientForServiceName("adventure");
            
            //assert
            Assert.False(ReferenceEquals(client2, client));
        }

        #endregion

        #region MakeGetServiceRequest

        [Fact]
        public async void MakeGetServiceRequest_NoToken_NoTokenHeader()
        {
            //arrange
            var manager = CreateServiceManager();
            Seed(manager);
            var request = new Request()
            {
                ServiceName = _testServiceName,
                EndPoint = "get"
            };

            //act
            var client = manager.GetClientForServiceName(_testServiceName);
            var response = await manager.MakeGetServiceRequest(request);
            
            //assert
            var obj = client.Deserialize<HttpBinResponse>(response);
            Assert.True(response.IsSuccessful);
            Assert.Null(obj.Data.Headers.Authorization);
        }
        
        [Fact]
        public async void MakeGetServiceRequest_NoParameters_NoParameters()
        {
            //arrange
            var manager = CreateServiceManager();
            Seed(manager);
            var request = new Request()
            {
                ServiceName = _testServiceName,
                EndPoint = "get"
            };

            //act
            var client = manager.GetClientForServiceName(_testServiceName);
            var response = await manager.MakeGetServiceRequest(request);
            
            //assert
            var obj = client.Deserialize<HttpBinResponse>(response);
            Assert.True(response.IsSuccessful);
            Assert.Empty(obj.Data.Args);
        }

        [Fact]
        public async void MakeGetServiceRequest_TokenAndParameters_ParametersAndToken()
        {
            //arrange
            var manager = CreateServiceManager();
            Seed(manager);
            var request = new Request()
            {
                ServiceName = _testServiceName,
                EndPoint = "get",
                Token = "BearerToken",
                Parameters = new Request()
                {
                    ServiceName = "foo",
                    EndPoint = "bar"
                }
            };

            //act
            var client = manager.GetClientForServiceName(_testServiceName);
            var response = await manager.MakeGetServiceRequest(request);
            
            //assert
            var obj = client.Deserialize<HttpBinResponse>(response);
            Assert.True(response.IsSuccessful);
            Assert.Equal("foo", obj.Data.Args["ServiceName"]);
            Assert.Equal("bar", obj.Data.Args["EndPoint"]);
            Assert.Equal("Bearer BearerToken", obj.Data.Headers.Authorization);
        }

        #endregion

        #region MakePostServiceRequestNoAuth

        [Fact]
        public async void MakePostServiceRequestNoAuth_DataPosted()
        {
            //arrange
            var manager = CreateServiceManager();
            Seed(manager);
            var request = new Request()
            {
                ServiceName = _testServiceName,
                EndPoint = "post",
                PostData = new Request()
                {
                    ServiceName = "foo",
                    EndPoint = "bar"
                }
            };

            //act
            var client = manager.GetClientForServiceName(_testServiceName);
            var response = await manager.MakePostServiceRequestNoAuth(request);
            
            //assert
            var obj = client.Deserialize<HttpBinResponse>(response);
            Assert.True(response.IsSuccessful);
            Assert.Equal("foo", obj.Data.Json.ServiceName);
            Assert.Equal("bar", obj.Data.Json.EndPoint);
        }

        #endregion
    }
}