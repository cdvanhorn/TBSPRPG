using System.Threading.Tasks;
using TbspRpgLib.InterServiceCommunication.RequestModels;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IGameServiceLink {
        Task<IscResponse> GetGames(Credentials creds);
        Task<IscResponse> CR_GetGames(Credentials creds);
        Task<IscResponse> StartGame(GameRequest requestData, Credentials creds);
        Task<IscResponse> CR_StartGame(GameRequest requestData, Credentials creds);
        Task<IscResponse> GetGameForAdventure(GameRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetGameForAdventure(GameRequest requestData, Credentials creds);
    }

    public class GameServiceLink : BaseServiceLink, IGameServiceLink {
        public GameServiceLink(IServiceCommunication serviceCommuncation) : base(serviceCommuncation) { }

        public async Task<IscResponse> GetGames(Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "game",
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
            var response = await _serviceCommunication.MakeRequestForUser(
                "game",
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
            var response = await _serviceCommunication.MakeRequestForUser(
                "game",
                $"games/{requestData.AdventureId}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetGameForAdventure(GameRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetGameForAdventure(requestData, creds);
        }
    }
}