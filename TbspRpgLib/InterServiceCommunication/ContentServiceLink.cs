using System.Threading.Tasks;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IContentServiceLink {
        Task<IscResponse> GetContentForGame(string gameId, Credentials creds);
        Task<IscResponse> CR_GetContentForGame(string gameId, Credentials creds);
        Task<IscResponse> GetLatestContentForGame(string gameId, Credentials creds);
        Task<IscResponse> CR_GetLastestContentForGame(string gameId, Credentials creds);
        //have to do the filter methods
    }

    public class ContentServiceLink : BaseServiceLink, IContentServiceLink {
        public ContentServiceLink(IServiceCommunication serviceCommuncation) : base(serviceCommuncation) { }

        public async Task<IscResponse> GetContentForGame(string gameId, Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "content",
                $"content/{gameId}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetContentForGame(string gameId, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetContentForGame(gameId, creds);
        }

        public async Task<IscResponse> GetLatestContentForGame(string gameId, Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "content",
                $"content/latests/{gameId}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetLastestContentForGame(string gameId, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetLatestContentForGame(gameId, creds);
        }
    }
}