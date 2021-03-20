using System.Dynamic;
using System.Threading.Tasks;
using TbspRpgLib.InterServiceCommunication.RequestModels;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IUserServiceLink {
        Task<IscResponse> Authenticate(UserRequest requestData);
        Task<IscResponse> GetUsers(Credentials creds);
        Task<IscResponse> CR_GetUsers(Credentials creds);
    }

    public class UserServiceLink : BaseServiceLink, IUserServiceLink {

        public UserServiceLink(IServiceCommunication serviceCommunication) : base(serviceCommunication){
        }

        public async Task<IscResponse> Authenticate(UserRequest requestData) {
            var response = await _serviceCommunication.MakePostNoAuth(
                "user",
                "users/authenticate",
                requestData
            );
            return new IscResponse() { Response = response };
        }

        public async Task<IscResponse> GetUsers(Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "user", 
                "users",
                creds.UserId);
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetUsers(Credentials creds) {
            PrepareControllerRequest(creds);
            return GetUsers(creds);
        }
    }
}