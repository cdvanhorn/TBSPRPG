using System.Threading.Tasks;
using RestSharp;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.InterServiceCommunication.Utilities;

namespace TbspRpgLib.InterServiceCommunication {
    public class BaseServiceLink {
        private readonly ITokenManager _tokenManager;
        private readonly IServiceManager _serviceManager;

        public BaseServiceLink(ITokenManager tokenManager, IServiceManager serviceManager)
        {
            _tokenManager = tokenManager;
            _serviceManager = serviceManager;
        }
        
        protected string ServiceName { get; set; }

        protected void PrepareControllerRequest(Credentials creds) {
            AddJwtTokenForUser(creds);
            DisableServiceCache();
        }

        protected static IscResponse ReturnResponse(IRestResponse response) {
            return new IscResponse()
            {
                Content = response.Content,
                StatusCode = (int)response.StatusCode,
                IsSuccessful = response.IsSuccessful,
                ErrorMessage = response.ErrorMessage
            };
        }

        protected void AddJwtTokenForUser(Credentials creds) {
            _tokenManager.AddTokenForUserId(creds.UserId, creds.Token);
        }

        protected void DisableServiceCache()
        {
            _serviceManager.CacheService = false;
        }
        
        protected async Task<IRestResponse> MakeRequestForUser(string endPoint, string userId) {
            var token = _tokenManager.GetTokenForUserId(userId);
            return await _serviceManager.MakeGetServiceRequest(new Request()
            {
                ServiceName = ServiceName,
                EndPoint = endPoint,
                Token = token
            });
        }

        protected async Task<IRestResponse> MakeRequestForUser(string endPoint, string userId, object parameters) {
            var token = _tokenManager.GetTokenForUserId(userId);
            return await _serviceManager.MakeGetServiceRequest(new Request()
            {
                ServiceName = ServiceName,
                EndPoint = endPoint,
                Token = token,
                Parameters = parameters
            });
        }

        protected async Task<IRestResponse> MakePostNoAuth(string endPoint, dynamic postData) {
            return await _serviceManager.MakePostServiceRequestNoAuth(new Request()
            {
                ServiceName = ServiceName,
                EndPoint = endPoint,
                PostData = postData
            });
        }
    }
}