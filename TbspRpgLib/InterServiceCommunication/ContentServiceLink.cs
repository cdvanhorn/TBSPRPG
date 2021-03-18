using System.Threading.Tasks;
using TbspRpgLib.InterServiceCommunication.RequestModels;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IContentServiceLink {
        Task<IscResponse> GetContentForGame(ContentRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetContentForGame(ContentRequest requestData, Credentials creds);
        Task<IscResponse> GetLatestContentForGame(ContentRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetLastestContentForGame(ContentRequest requestData, Credentials creds);
        Task<IscResponse> FilterContent(ContentFilterRequest requestData, Credentials creds);
        Task<IscResponse> CR_FilterContent(ContentFilterRequest requestData, Credentials creds);
    }

    public class ContentServiceLink : BaseServiceLink, IContentServiceLink {
        public ContentServiceLink(IServiceCommunication serviceCommuncation) : base(serviceCommuncation) { }

        public async Task<IscResponse> GetContentForGame(ContentRequest requestData, Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "content",
                $"content/{requestData.GameId}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetContentForGame(ContentRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetContentForGame(requestData, creds);
        }

        public async Task<IscResponse> GetLatestContentForGame(ContentRequest requestData, Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "content",
                $"content/latest/{requestData.GameId}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetLastestContentForGame(ContentRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetLatestContentForGame(requestData, creds);
        }

        public async Task<IscResponse> FilterContent(ContentFilterRequest requestData, Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "content",
                $"content/filter/{requestData.GameId}",
                creds.UserId,
                requestData
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_FilterContent(ContentFilterRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return FilterContent(requestData, creds);
        }
    }
}