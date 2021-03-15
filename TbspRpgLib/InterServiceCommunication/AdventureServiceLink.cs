using System.Threading.Tasks;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IAdventureServiceLink {
        Task<IscResponse> GetInitialLocation(string adventureId, Credentials creds);
        Task<IscResponse> CR_GetInitialLocation(string adventureId, Credentials creds);
        Task<IscResponse> GetAdventureByName(string name, Credentials creds);
        Task<IscResponse> CR_GetAdventureByName(string name, Credentials creds);
        Task<IscResponse> GetAdventures(Credentials creds);
        Task<IscResponse> CR_GetAdventures(Credentials creds);
    }

    public class AdventureServiceLink : BaseServiceLink, IAdventureServiceLink {
        public AdventureServiceLink(IServiceCommunication serviceCommuncation) : base(serviceCommuncation) { }

        public async Task<IscResponse> GetInitialLocation(string adventureId, Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "adventure", 
                $"adventures/initiallocation/{adventureId}",
                creds.UserId);
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetInitialLocation(string adventureId, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetInitialLocation(adventureId, creds);
        }

        public async Task<IscResponse> GetAdventureByName(string name, Credentials creds) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "adventure",
                $"adventures/{name}",
                creds.UserId
            );
            return ReturnResponse(response);
        }

        public Task<IscResponse> CR_GetAdventureByName(string name, Credentials creds) {
            PrepareControllerRequest(creds);
            return GetAdventureByName(name, creds);
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