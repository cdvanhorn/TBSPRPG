using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using TbspRpgLib.Entities;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.Services;

namespace TbspRpgLib.InterServiceCommunication.Utilities
{
    public interface IServiceManager
    {
        bool CacheService { get; set; }
        RestClient GetClientForServiceName(string serviceName);
        void AddClientForServiceName(string serviceName, RestClient client);
        Task<IRestResponse> MakeGetServiceRequest(Request request);
        Task<IRestResponse> MakePostServiceRequestNoAuth(Request request);
    }
    
    public class ServiceManager : IServiceManager
    {
        private static readonly ConcurrentDictionary<string, RestClient> _clients
            = new ConcurrentDictionary<string, RestClient>();
        private readonly IServiceService _serviceService;

        public ServiceManager(IServiceService serviceService)
        {
            _serviceService = serviceService;
            CacheService = true;
        }
        
        public bool CacheService { get; set; }
        
        private RestClient CreateRestClient(string serviceName) {
            var service = _serviceService.GetServiceByName(serviceName);
            if(service == null)
                throw new ArgumentException($"invalid service name {serviceName}");
            return new RestClient(service.Url);
        }

        public void AddClientForServiceName(string serviceName, RestClient client)
        {
            _clients[serviceName] = client;
        }

        public RestClient GetClientForServiceName(string serviceName) {
            if(!CacheService)
                return CreateRestClient(serviceName);

            if(!_clients.ContainsKey(serviceName)) {
                AddClientForServiceName(serviceName, CreateRestClient(serviceName));
            }
            return _clients[serviceName];
        }
        
        public Task<IRestResponse> MakeGetServiceRequest(Request request) {
            var restShareRequest = new RestRequest(request.EndPoint, DataFormat.Json);

            //if jwtToken is null assume no authorization
            if(request.Token != null)
                restShareRequest.AddHeader("Authorization", $"Bearer {request.Token}");
            
            if(request.Parameters != null) {
                foreach(var propertyInfo in request.Parameters.GetType().GetProperties()) {
                    if(propertyInfo.GetValue(request.Parameters) != null)
                        restShareRequest.AddParameter(propertyInfo.Name, propertyInfo.GetValue(request.Parameters));
                }
            }
            
            //let's make the request
            var client = GetClientForServiceName(request.ServiceName);
            return client.ExecuteGetAsync(restShareRequest);
        }

        public  Task<IRestResponse> MakePostServiceRequestNoAuth(Request request) {
            var restShareRequest = new RestRequest(request.EndPoint, DataFormat.Json);

            //add the post data
            restShareRequest.AddJsonBody(request.PostData);

            //make the request
            var client = GetClientForServiceName(request.ServiceName);
            return client.ExecutePostAsync(restShareRequest);
        }
    }
}