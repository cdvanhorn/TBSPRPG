using System.Threading.Tasks;
using TbspRpgLib.InterServiceCommunication.RequestModels;
using TbspRpgLib.InterServiceCommunication.Utilities;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IAdventureServiceLink {
        Task<IscResponse> GetInitialLocation(AdventureRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetInitialLocation(AdventureRequest requestData, Credentials creds);
        Task<IscResponse> GetAdventureByName(AdventureRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetAdventureByName(AdventureRequest requestData, Credentials creds);
        Task<IscResponse> GetAdventureById(AdventureRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetAdventureById(AdventureRequest requestData, Credentials creds);
        Task<IscResponse> GetAdventures(Credentials creds);
        Task<IscResponse> CR_GetAdventures(Credentials creds);
        Task<IscResponse> GetRoutesForLocation(AdventureRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetRoutesForLocation(AdventureRequest requestData, Credentials creds);
        Task<IscResponse> GetSourceForKey(AdventureRequest adventureRequest, Credentials creds);
        Task<IscResponse> CR_GetSourceForKey(AdventureRequest adventureRequest, Credentials creds);
    }

    public class AdventureServiceLink : BaseServiceLink, IAdventureServiceLink {
        public AdventureServiceLink(ITokenManager tokenManager, IServiceManager serviceManager) :
            base(tokenManager, serviceManager)
        {
            ServiceName = "adventure";
        }

        public async Task<IscResponse> GetInitialLocation(AdventureRequest requestData, Credentials creds) {
            var response = await MakeRequestForUser(
                $"adventures/initiallocation/{requestData.Id}",
                creds.UserId);
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetInitialLocation(AdventureRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetInitialLocation(requestData, creds);
        }

        public async Task<IscResponse> GetAdventureByName(AdventureRequest requestData, Credentials creds) {
            var response = await MakeRequestForUser(
                $"adventures/{requestData.Name}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetAdventureByName(AdventureRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetAdventureByName(requestData, creds);
        }
        
        public async Task<IscResponse> GetAdventureById(AdventureRequest requestData, Credentials creds) {
            var response = await MakeRequestForUser(
                $"adventures/{requestData.Id}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetAdventureById(AdventureRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetAdventureByName(requestData, creds);
        }

        public async Task<IscResponse> GetAdventures(Credentials creds) {
            var response = await MakeRequestForUser(
                "adventures",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetAdventures(Credentials creds) {
            PrepareControllerRequest(creds);
            return GetAdventures(creds);
        }
        
        public async Task<IscResponse> GetRoutesForLocation(AdventureRequest requestData, Credentials creds)
        {
            var response = await MakeRequestForUser(
                $"locations/routes/{requestData.LocationId}",
                creds.UserId);
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetRoutesForLocation(AdventureRequest requestData, Credentials creds)
        {
            PrepareControllerRequest(creds);
            return GetRoutesForLocation(requestData, creds);
        }

        public async Task<IscResponse> GetSourceForKey(AdventureRequest adventureRequest, Credentials creds)
        {
            var response = await MakeRequestForUser(
                $"sources/{adventureRequest.Language}/{adventureRequest.SourceKey}",
                creds.UserId);
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetSourceForKey(AdventureRequest adventureRequest, Credentials creds)
        {
            PrepareControllerRequest(creds);
            return GetSourceForKey(adventureRequest, creds);
        }
    }
}