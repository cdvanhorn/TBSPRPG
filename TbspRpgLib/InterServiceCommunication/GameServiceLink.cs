using System.Threading.Tasks;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IGameServiceLink {
        Task<IscResponse> GetGames(Credentials creds);
        Task<IscResponse> CR_GetGames(Credentials creds);
        Task<IscResponse> StartGame(string adventureName, Credentials creds);
        Task<IscResponse> CR_StartGame(string adventureName, Credentials creds);
        Task<IscResponse> GetGameForAdventure(string adventureName, Credentials creds);
        Task<IscResponse> CR_GetGameForAdventure(string adventureName, Credentials creds);
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

        public async Task<IscResponse> StartGame(string adventureName, Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "game",
                $"games/start/{adventureName}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_StartGame(string adventureName, Credentials creds) {
            PrepareControllerRequest(creds);
            return StartGame(adventureName, creds);
        }

        public async Task<IscResponse> GetGameForAdventure(string adventureName, Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "game",
                $"games/{adventureName}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetGameForAdventure(string adventureName, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetGameForAdventure(adventureName, creds);
        }
    }
}