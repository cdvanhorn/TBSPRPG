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
        bool CacheService { get; set; }
    }

    public class ServiceCommunication : IServiceCommunication {
        private readonly Dictionary<string, RestClient> _clients;
        private readonly IServiceService _serviceService;
        private readonly ITokenManager _tokenManager;

        public ServiceCommunication(IServiceService serviceService, ITokenManager tokenManager) {
            _clients = new Dictionary<string, RestClient>();
            _serviceService = serviceService;
            _tokenManager = tokenManager;
            CacheService = true;
        }

        public void AddTokenForUserId(string userId, string token)
        {
            _tokenManager.AddTokenForUserId(userId, token);
        }

        public bool CacheService { get; set; }

        private RestClient CreateRestClient(string serviceName) {
            Service service = _serviceService.GetServiceByName(serviceName);
            if(service == null)
                throw new ArgumentException($"invalid service name {serviceName}");
            Console.WriteLine($"creating service with url {service.Url}");
            return new RestClient(service.Url);
        }

        private RestClient GetClientForServiceName(string serviceName) {
            if(!CacheService)
                return CreateRestClient(serviceName);

            if(!_clients.ContainsKey(serviceName)) {
                //create the client
                _clients.Add(serviceName, CreateRestClient(serviceName));
            }
            return _clients[serviceName];
        }

        public async Task<IRestResponse> MakeRequestForUser(string serviceName, string endPoint, string userId) {
            var clientTask = GetClientForServiceName(serviceName);
            var token = _tokenManager.GetTokenForUserId(userId);
            return await MakeGetServiceRequest(clientTask, endPoint, token, null);
        }

        public async Task<IRestResponse> MakeRequestForUser(string serviceName, string endPoint, string userId, object parameters) {
            var clientTask = GetClientForServiceName(serviceName);
            var token = _tokenManager.GetTokenForUserId(userId);
            return await MakeGetServiceRequest(clientTask, endPoint, token, parameters);
        }

        public async Task<IRestResponse> MakePostNoAuth(string serviceName, string endPoint, dynamic postData) {
            var clientTask = GetClientForServiceName(serviceName);
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