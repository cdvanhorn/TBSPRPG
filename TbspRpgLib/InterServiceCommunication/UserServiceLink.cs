using System.Dynamic;
using System.Threading.Tasks;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.InterServiceCommunication.Utilities;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IUserServiceLink {
        Task<IscResponse> Authenticate(UserRequest requestData);
        Task<IscResponse> GetUsers(Credentials creds);
        Task<IscResponse> CR_GetUsers(Credentials creds);
    }

    public class UserServiceLink : BaseServiceLink, IUserServiceLink {

        public UserServiceLink(ITokenManager tokenManager, IServiceManager serviceManager) : 
            base(tokenManager, serviceManager)
        {
            ServiceName = "user";
        }

        public async Task<IscResponse> Authenticate(UserRequest requestData) {
            var response = await MakePostNoAuth(
                "users/authenticate",
                requestData
            );
            return new IscResponse() { Response = response };
        }

        public async Task<IscResponse> GetUsers(Credentials creds) {
            var response = await MakeRequestForUser(
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