using System.Dynamic;
using System.Threading.Tasks;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IUserServiceLink {
        Task<IscResponse> Authenticate(string userName, string password);
        Task<IscResponse> GetUsers(string userId);
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

        public async Task<IscResponse> GetUsers(string userId) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "user", 
                "users",
                userId);
            return new IscResponse() { Response = response };
        }
    }
}