namespace TbspRpgLib.InterServiceCommunication {
    public class BaseServiceLink {
        protected IServiceCommunication _serviceCommunication;

        public BaseServiceLink(IServiceCommunication serviceCommuncation) {
            _serviceCommunication = serviceCommuncation;
        }

        public void AddJwtTokenForUser(string userId, string token) {
            _serviceCommunication.AddTokenForUserId(userId, token);
        }
    }
}