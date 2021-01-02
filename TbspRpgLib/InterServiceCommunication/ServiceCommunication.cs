using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using RestSharp;

using TbspRpgLib.Entities;
using TbspRpgLib.Services;
using TbspRpgLib.Jwt;
using TbspRpgLib.Settings;

namespace TbspRpgLib.InterServiceCommunication {
    public class ServiceCommunication {
        private Dictionary<string, RestClient> _clients;
        private Dictionary<string, string> _tokens;
        private IServiceService _serviceService;
        private IJwtHelper _jwtHelper;

        public ServiceCommunication(IServiceService serviceService, IJwtSettings jwtSettings) {
            _clients = new Dictionary<string, RestClient>();
            _tokens = new Dictionary<string, string>();
            _serviceService = serviceService;
            _jwtHelper = new JwtHelper(jwtSettings.Secret);
        }

        private async Task<RestClient> GetClientForServiceName(string serviceName) {
            if(!_clients.ContainsKey(serviceName)) {
                //create the client
                Service service = await _serviceService.GetServiceByName(serviceName);
                if(service == null)
                    throw new ArgumentException($"invalid service name {serviceName}");
                _clients.Add(serviceName, new RestClient(service.Url));
            }
            return _clients[serviceName];
        }

        private string GetTokenForUserId(string userId) {
            if(!_tokens.ContainsKey(userId)) {
                _tokens.Add(userId, _jwtHelper.GenerateToken(userId));
            }
            return _tokens[userId];
        }

        //need to figure out the end point in the public api
        //produce a jwt token, maybe maintain a hash of jwt tokens in this
        //library, maybe a cache of 100
        //make a call to the endpoint
        //return response
        //should I provide convenience methods (that could be a lot of methods)?
        //should the public api use this library?
        public async void GetInitialLocation(string adventureId, string userId) {
            //string url = {http://localhost:8000/api}/adventures/initiallocation/{adventureId}
            //url would actually be {http://adventureapi:8002/api}/adventures/initiallocation/{adventureId}
            //look up jwt token for user id, if there isn't one generate one and cache it
            //each service will end up with it's own cache in memory
            //need the service name to find the base url
            //then I believe a request can be made
            var response = await MakeRequestForUser(
                "adventure", 
                $"adventures/initiallocation{adventureId}",
                userId);
            //check if the response was successfull, if so return the location id
            //if unsucessful 
        }

        protected async Task<IRestResponse> MakeRequestForUser(string serviceName, string endPoint, string userId) {
            var clientTask = GetClientForServiceName(serviceName);
            var token = GetTokenForUserId(userId);
            return await MakeGetServiceRequest(await clientTask, endPoint, token);
        }

        private Task<IRestResponse> MakeGetServiceRequest(RestClient client, string endPoint, string jwtToken) {
            var request = new RestRequest(endPoint, DataFormat.Json);

            //if jwtToken is null assume no authorization
            if(jwtToken != null)
                request.AddHeader("Authorization", $"Bearer {jwtToken}");
            
            //let's make the request
            return client.ExecuteGetAsync(request);
        }
    }
}