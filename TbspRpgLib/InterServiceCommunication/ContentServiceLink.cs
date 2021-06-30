using System.Text;
using System.Threading.Tasks;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.InterServiceCommunication.Utilities;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IContentServiceLink {
        Task<IscResponse> GetContentForGame(ContentRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetContentForGame(ContentRequest requestData, Credentials creds);
        Task<IscResponse> GetLatestContentForGame(ContentRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetLatestContentForGame(ContentRequest requestData, Credentials creds);
        Task<IscResponse> FilterContent(ContentFilterRequest requestData, Credentials creds);
        Task<IscResponse> CR_FilterContent(ContentFilterRequest requestData, Credentials creds);
        Task<IscResponse> GetSourceContent(ContentRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetSourceContent(ContentRequest requestData, Credentials creds);
        Task<IscResponse> GetContentForGameAfterPosition(ContentRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetContentForGameAfterPosition(ContentRequest requestData, Credentials creds);
    }

    public class ContentServiceLink : BaseServiceLink, IContentServiceLink {
        public ContentServiceLink(ITokenManager tokenManager, IServiceManager serviceManager) :
            base(tokenManager, serviceManager)
        {
            ServiceName = "content";
        }

        public async Task<IscResponse> GetContentForGame(ContentRequest requestData, Credentials creds) {
            var response = await MakeRequestForUser(
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
            var response = await MakeRequestForUser(
                $"content/latest/{requestData.GameId}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetLatestContentForGame(ContentRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetLatestContentForGame(requestData, creds);
        }

        public async Task<IscResponse> FilterContent(ContentFilterRequest requestData, Credentials creds) {
            var response = await MakeRequestForUser(
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

        public async Task<IscResponse> GetSourceContent(ContentRequest requestData, Credentials creds)
        {
            var url = new StringBuilder();
            url.Append("content/source/");
            if (requestData.Language != null)
                url.Append($"{requestData.Language}/");
            else if (requestData.GameId != null)
                url.Append($"{requestData.GameId}/");
            url.Append(requestData.SourceKey);

            var response = await MakeRequestForUser(
                url.ToString(),
                creds.UserId);
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetSourceContent(ContentRequest requestData, Credentials creds)
        {
            PrepareControllerRequest(creds);
            return GetSourceContent(requestData, creds);
        }

        public async Task<IscResponse> GetContentForGameAfterPosition(ContentRequest requestData, Credentials creds)
        {
            var response = await MakeRequestForUser(
                $"content/{requestData.GameId}/after/{requestData.Position}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetContentForGameAfterPosition(ContentRequest requestData, Credentials creds)
        {
            PrepareControllerRequest(creds);
            return GetContentForGameAfterPosition(requestData, creds);
        }
    }
}