using System.Dynamic;
using System.Threading.Tasks;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IUserServiceLink {
        Task<IscResponse> Authenticate(string userName, string password);
        Task<IscResponse> GetUsers(Credentials creds);
        Task<IscResponse> CR_GetUsers(Credentials creds);
    }

    public class UserServiceLink : BaseServiceLink, IUserServiceLink {

        public UserServiceLink(IServiceCommunication serviceCommunication) : base(serviceCommunication){
        }

        public async Task<IscResponse> Authenticate(string userName, string password) {
            dynamic postData = new ExpandoObject();
            postData.Username = userName;
            postData.Password = password;

            var response = await _serviceCommunication.MakePostNoAuth(
                "user",
                "users/authenticate",
                postData
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