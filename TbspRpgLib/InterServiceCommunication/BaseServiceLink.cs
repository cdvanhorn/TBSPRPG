using RestSharp;

namespace TbspRpgLib.InterServiceCommunication {
    public class BaseServiceLink {
        protected IServiceCommunication _serviceCommunication;

        public BaseServiceLink(IServiceCommunication serviceCommuncation) {
            _serviceCommunication = serviceCommuncation;
        }

        public void PrepareControllerRequest(Credentials creds) {
            AddJwtTokenForUser(creds);
            DisableServiceCache();
        }

        public IscResponse ReturnResponse(IRestResponse response) {
            return new IscResponse() { Response = response };
        }

        public void AddJwtTokenForUser(Credentials creds) {
            _serviceCommunication.AddTokenForUserId(creds.UserId, creds.Token);
        }

        public void DisableServiceCache() {
            _serviceCommunication.CacheService = false;
        }
    }
}