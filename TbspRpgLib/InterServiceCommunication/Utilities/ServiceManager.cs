using System;
using System.Collections.Generic;
using RestSharp;
using TbspRpgLib.Entities;
using TbspRpgLib.Services;

namespace TbspRpgLib.InterServiceCommunication.Utilities
{
    public interface IServiceManager
    {
        bool CacheService { get; set; }
        RestClient GetClientForServiceName(string serviceName);
    }
    
    public class ServiceManager : IServiceManager
    {
        private readonly Dictionary<string, RestClient> _clients;
        private readonly IServiceService _serviceService;

        public ServiceManager(IServiceService serviceService)
        {
            _clients = new Dictionary<string, RestClient>();
            _serviceService = serviceService;
        }
        
        public bool CacheService { get; set; }
        
        private RestClient CreateRestClient(string serviceName) {
            var service = _serviceService.GetServiceByName(serviceName);
            if(service == null)
                throw new ArgumentException($"invalid service name {serviceName}");
            Console.WriteLine($"creating service with url {service.Url}");
            return new RestClient(service.Url);
        }

        public RestClient GetClientForServiceName(string serviceName) {
            if(!CacheService)
                return CreateRestClient(serviceName);

            if(!_clients.ContainsKey(serviceName)) {
                //create the client
                _clients.Add(serviceName, CreateRestClient(serviceName));
            }
            return _clients[serviceName];
        }
    }
}