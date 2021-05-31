using RestSharp;
using TbspRpgLib.InterServiceCommunication.Utilities;

namespace TbspRpgLib.InterServiceCommunication {
    public class BaseServiceLink {
        protected readonly IServiceCommunication _serviceCommunication;

        public BaseServiceLink(IServiceCommunication serviceCommunication) {
            _serviceCommunication = serviceCommunication;
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
            _serviceCommunication.DisableServiceCache();
        }
    }
}