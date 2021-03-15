using System.Threading.Tasks;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IMapServiceLink {
        Task<IscResponse> GetLocations(Credentials creds);
        Task<IscResponse> CR_GetLocations(Credentials creds);
        Task<IscResponse> GetLocationByGameId(string gameId, Credentials creds);
        Task<IscResponse> CR_GetLocationByGameId(string gameId, Credentials creds);
    }

    public class MapServiceLink : BaseServiceLink, IMapServiceLink {
        public MapServiceLink(IServiceCommunication serviceCommuncation) : base(serviceCommuncation) { }

        public async Task<IscResponse> GetLocations(Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "map",
                "locations",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetLocations(Credentials creds) {
            PrepareControllerRequest(creds);
            return GetLocations(creds);
        }

        public async Task<IscResponse> GetLocationByGameId(string gameId, Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "map",
                $"locations/{gameId}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetLocationByGameId(string gameId, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetLocationByGameId(gameId, creds);
        }
    }
}