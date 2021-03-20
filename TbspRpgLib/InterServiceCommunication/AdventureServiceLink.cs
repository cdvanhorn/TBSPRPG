using System.Threading.Tasks;
using TbspRpgLib.InterServiceCommunication.RequestModels;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IAdventureServiceLink {
        Task<IscResponse> GetInitialLocation(AdventureRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetInitialLocation(AdventureRequest requestData, Credentials creds);
        Task<IscResponse> GetAdventureByName(AdventureRequest requestData, Credentials creds);
        Task<IscResponse> CR_GetAdventureByName(AdventureRequest requestData, Credentials creds);
        Task<IscResponse> GetAdventures(Credentials creds);
        Task<IscResponse> CR_GetAdventures(Credentials creds);
    }

    public class AdventureServiceLink : BaseServiceLink, IAdventureServiceLink {
        public AdventureServiceLink(IServiceCommunication serviceCommuncation) : base(serviceCommuncation) { }

        public async Task<IscResponse> GetInitialLocation(AdventureRequest requestData, Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "adventure", 
                $"adventures/initiallocation/{requestData.Id}",
                creds.UserId);
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetInitialLocation(AdventureRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetInitialLocation(requestData, creds);
        }

        public async Task<IscResponse> GetAdventureByName(AdventureRequest requestData, Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "adventure",
                $"adventures/{requestData.Name}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetAdventureByName(AdventureRequest requestData, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetAdventureByName(requestData, creds);
        }

        public async Task<IscResponse> GetAdventures(Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "adventure",
                "adventures",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetAdventures(Credentials creds) {
            PrepareControllerRequest(creds);
            return GetAdventures(creds);
        }
    }
}