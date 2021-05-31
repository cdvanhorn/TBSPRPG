using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using RestSharp;

using TbspRpgLib.Entities;
using TbspRpgLib.InterServiceCommunication.Utilities;
using TbspRpgLib.Services;
using TbspRpgLib.Jwt;
using TbspRpgLib.Settings;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IServiceCommunication {
        Task<IRestResponse> MakeRequestForUser(string serviceName, string endPoint, string userId);
        Task<IRestResponse> MakeRequestForUser(string serviceName, string endPoint, string userId, object parameters);
        Task<IRestResponse> MakePostNoAuth(string serviceName, string endPoint, object postData);
        void AddTokenForUserId(string userId, string token);
        void DisableServiceCache();
    }

    public class ServiceCommunication : IServiceCommunication {
        private readonly ITokenManager _tokenManager;
        private readonly IServiceManager _serviceManager;

        public ServiceCommunication(ITokenManager tokenManager, IServiceManager serviceManager) {
            _tokenManager = tokenManager;
            _serviceManager = serviceManager;
        }

        public void AddTokenForUserId(string userId, string token)
        {
            _tokenManager.AddTokenForUserId(userId, token);
        }

        public void DisableServiceCache()
        {
            _serviceManager.CacheService = false;
        }

        public async Task<IRestResponse> MakeRequestForUser(string serviceName, string endPoint, string userId) {
            var clientTask = _serviceManager.GetClientForServiceName(serviceName);
            var token = _tokenManager.GetTokenForUserId(userId);
            return await MakeGetServiceRequest(clientTask, endPoint, token, null);
        }

        public async Task<IRestResponse> MakeRequestForUser(string serviceName, string endPoint, string userId, object parameters) {
            var clientTask = _serviceManager.GetClientForServiceName(serviceName);
            var token = _tokenManager.GetTokenForUserId(userId);
            return await MakeGetServiceRequest(clientTask, endPoint, token, parameters);
        }

        public async Task<IRestResponse> MakePostNoAuth(string serviceName, string endPoint, dynamic postData) {
            var clientTask = _serviceManager.GetClientForServiceName(serviceName);
            return await MakePostServiceRequestNoAuth(clientTask, endPoint, postData);
        }

        private Task<IRestResponse> MakeGetServiceRequest(RestClient client, string endPoint, string jwtToken, object parameters) {
            var request = new RestRequest(endPoint, DataFormat.Json);
            Console.WriteLine($"calling endpoint {endPoint}");

            //if jwtToken is null assume no authorization
            if(jwtToken != null)
                request.AddHeader("Authorization", $"Bearer {jwtToken}");
            
            if(parameters != null) {
                foreach(var propertyInfo in parameters.GetType().GetProperties()) {
                    if(propertyInfo.GetValue(parameters) != null)
                        request.AddParameter(propertyInfo.Name, propertyInfo.GetValue(parameters));
                }
            }
            
            //let's make the request
            return client.ExecuteGetAsync(request);
        }

        private Task<IRestResponse> MakePostServiceRequestNoAuth(RestClient client, string endPoint, object postData) {
            var request = new RestRequest(endPoint, DataFormat.Json);
            Console.WriteLine($"calling endpoint {endPoint}");

            //add the post data
            request.AddJsonBody(postData);

            //make the request
            return client.ExecutePostAsync(request);
        }
    }
}