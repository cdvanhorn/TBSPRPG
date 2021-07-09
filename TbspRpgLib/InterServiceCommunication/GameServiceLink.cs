using System.Threading.Tasks;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.InterServiceCommunication.Utilities;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IGameServiceLink {
        Task<IscResponse> GetGames(Credentials creds);
        Task<IscResponse> CR_GetGames(Credentials creds);
        Task<IscResponse> StartGame(GameRequest requestData, Credentials creds);
        Task<IscResponse> CR_StartGame(GameRequest requestData, Credentials creds);
        Task<IscResponse> GetGameForAdventure(GameRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetGameForAdventure(GameRequest requestData, Credentials creds);
        Task<IscResponse> GetLatestContentForGame(GameRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetLatestContentForGame(GameRequest requestData, Credentials creds);
        Task<IscResponse> FilterContent(ContentFilterRequest requestData, Credentials creds);
        Task<IscResponse> CR_FilterContent(ContentFilterRequest requestData, Credentials creds);
        Task<IscResponse> GetContentForGameAfterPosition(GameRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetContentForGameAfterPosition(GameRequest requestData, Credentials creds);
    }

    public class GameServiceLink : BaseServiceLink, IGameServiceLink {
        public GameServiceLink(ITokenManager tokenManager, IServiceManager serviceManager) :
            base(tokenManager, serviceManager)
        {
            ServiceName = "game";
        }

        public async Task<IscResponse> GetGames(Credentials creds) {
            var response = await MakeRequestForUser(
                "games",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetGames(Credentials creds) {
            PrepareControllerRequest(creds);
            return GetGames(creds);
        }

        public async Task<IscResponse> StartGame(GameRequest requestData, Credentials creds) {
            var response = await MakeRequestForUser(
                $"games/start/{requestData.AdventureId}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_StartGame(GameRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return StartGame(requestData, creds);
        }

        public async Task<IscResponse> GetGameForAdventure(GameRequest requestData, Credentials creds) {
            var response = await MakeRequestForUser(
                $"games/{requestData.AdventureId}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetGameForAdventure(GameRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetGameForAdventure(requestData, creds);
        }

        public async Task<IscResponse> GetLatestContentForGame(GameRequest requestData, Credentials creds)
        {
            var response = await MakeRequestForUser(
                $"content/{requestData.GameId}/latest",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetLatestContentForGame(GameRequest requestData, Credentials creds)
        {
            PrepareControllerRequest(creds);
            return GetLatestContentForGame(requestData, creds);
        }

        public async Task<IscResponse> FilterContent(ContentFilterRequest requestData, Credentials creds) {
            var response = await MakeRequestForUser(
                $"content/{requestData.GameId}/filter",
                creds.UserId,
                requestData
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_FilterContent(ContentFilterRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return FilterContent(requestData, creds);
        }

        public async Task<IscResponse> GetContentForGameAfterPosition(GameRequest requestData, Credentials creds)
        {
            var response = await MakeRequestForUser(
                $"content/{requestData.GameId}/after/{requestData.Position}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetContentForGameAfterPosition(GameRequest requestData, Credentials creds)
        {
            PrepareControllerRequest(creds);
            return GetContentForGameAfterPosition(requestData, creds);
        }
    }
}