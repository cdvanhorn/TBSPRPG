using System.Threading.Tasks;

using RestSharp;

using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.InterServiceCommunication.Utilities;

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
            var token = _tokenManager.GetTokenForUserId(userId);
            return await _serviceManager.MakeGetServiceRequest(new Request()
            {
                ServiceName = serviceName,
                EndPoint = endPoint,
                Token = token
            });
        }

        public async Task<IRestResponse> MakeRequestForUser(string serviceName, string endPoint, string userId, object parameters) {
            var token = _tokenManager.GetTokenForUserId(userId);
            return await _serviceManager.MakeGetServiceRequest(new Request()
            {
                ServiceName = serviceName,
                EndPoint = endPoint,
                Token = token,
                Parameters = parameters
            });
        }

        public async Task<IRestResponse> MakePostNoAuth(string serviceName, string endPoint, dynamic postData) {
            return await _serviceManager.MakePostServiceRequestNoAuth(new Request()
            {
                ServiceName = serviceName,
                EndPoint = endPoint,
                PostData = postData
            });
        }
    }
}