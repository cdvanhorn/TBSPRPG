using System.Threading.Tasks;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.InterServiceCommunication.Utilities;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IMapServiceLink {
        Task<IscResponse> GetLocations(Credentials creds);
        Task<IscResponse> CR_GetLocations(Credentials creds);
        Task<IscResponse> GetLocationByGameId(MapRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetLocationByGameId(MapRequest requestData, Credentials creds);
        Task<IscResponse> GetRoutesForGame(MapRequest requestData, Credentials credentials);
        Task<IscResponse> CR_GetRoutesForGame(MapRequest requestData, Credentials credentials);
    }

    public class MapServiceLink : BaseServiceLink, IMapServiceLink {
        public MapServiceLink(ITokenManager tokenManager, IServiceManager serviceManager) :
            base(tokenManager, serviceManager)
        {
            ServiceName = "map";
        }

        public async Task<IscResponse> GetLocations(Credentials creds) {
            var response = await MakeRequestForUser(
                "locations",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetLocations(Credentials creds) {
            PrepareControllerRequest(creds);
            return GetLocations(creds);
        }

        public async Task<IscResponse> GetLocationByGameId(MapRequest requestData, Credentials creds) {
            var response = await MakeRequestForUser(
                $"locations/{requestData.GameId}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetLocationByGameId(MapRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetLocationByGameId(requestData, creds);
        }

        public async Task<IscResponse> GetRoutesForGame(MapRequest requestData, Credentials credentials)
        {
            var response = await MakeRequestForUser(
                $"routes/{requestData.GameId}",
                credentials.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetRoutesForGame(MapRequest requestData, Credentials credentials)
        {
            PrepareControllerRequest(credentials);
            return GetRoutesForGame(requestData, credentials);
        }
    }
}