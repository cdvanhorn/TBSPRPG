using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using RestSharp;

using TbspRpgLib.Entities;
using TbspRpgLib.Services;
using TbspRpgLib.Jwt;
using TbspRpgLib.Settings;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IServiceCommunication {
        Task<IRestResponse> MakeRequestForUser(string serviceName, string endPoint, string userId);
        Task<IRestResponse> MakePostNoAuth(string serviceName, string endPoint, dynamic postData);
        void AddTokenForUserId(string userId, string token);
        bool CacheService { get; set; }
    }

    public class ServiceCommunication : IServiceCommunication {
        private Dictionary<string, RestClient> _clients;
        private Dictionary<string, string> _tokens;
        private IServiceService _serviceService;
        private IJwtHelper _jwtHelper;

        public ServiceCommunication(IServiceService serviceService, IJwtSettings jwtSettings) {
            _clients = new Dictionary<string, RestClient>();
            _tokens = new Dictionary<string, string>();
            _serviceService = serviceService;
            _jwtHelper = new JwtHelper(jwtSettings.Secret);
            CacheService = true;
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

        private string GetTokenForUserId(string userId) {
            if(!_tokens.ContainsKey(userId)) {
                Console.WriteLine($"generating token for {userId}");
                _tokens.Add(userId, _jwtHelper.GenerateToken(userId));
            }
            Console.WriteLine($"getting token {_tokens[userId]}");
            return _tokens[userId];
        }

        public void AddTokenForUserId(string userId, string token) {
            if(!_tokens.ContainsKey(userId))
                _tokens.Add(userId, token);
            else
                _tokens[userId] = token;
        }

        public async Task<IRestResponse> MakeRequestForUser(string serviceName, string endPoint, string userId) {
            var clientTask = GetClientForServiceName(serviceName);
            var token = GetTokenForUserId(userId);
            return await MakeGetServiceRequest(clientTask, endPoint, token);
        }

        public async Task<IRestResponse> MakePostNoAuth(string serviceName, string endPoint, dynamic postData) {
            var clientTask = GetClientForServiceName(serviceName);
            return await MakePostServiceRequestNoAuth(clientTask, endPoint, postData);
        }

        private Task<IRestResponse> MakeGetServiceRequest(RestClient client, string endPoint, string jwtToken) {
            var request = new RestRequest(endPoint, DataFormat.Json);
            Console.WriteLine($"calling endpoint {endPoint}");

            //if jwtToken is null assume no authorization
            if(jwtToken != null)
                request.AddHeader("Authorization", $"Bearer {jwtToken}");
            
            //let's make the request
            return client.ExecuteGetAsync(request);
        }

        private Task<IRestResponse> MakePostServiceRequestNoAuth(RestClient client, string endPoint, dynamic postData) {
            var request = new RestRequest(endPoint, DataFormat.Json);
            Console.WriteLine($"calling endpoint {endPoint}");

            //add the post data
            request.AddJsonBody(postData);

            //make the request
            return client.ExecutePostAsync(request);
        }
    }
}