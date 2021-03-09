using System.Threading.Tasks;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IAdventureServiceLink {
        Task<IscResponse> GetInitialLocation(string adventureId, string userId);
    }

    public class AdventureServiceLink : IAdventureServiceLink {
        private IServiceCommunication _serviceCommunication;

        public AdventureServiceLink(IServiceCommunication serviceCommuncation) {
            _serviceCommunication = serviceCommuncation;
        }

        public async Task<IscResponse> GetInitialLocation(string adventureId, string userId) {
            var response = await _serviceCommunication.MakeRequestForUser(
                "adventure", 
                $"adventures/initiallocation/{adventureId}",
                userId);
            return new IscResponse() { Response = response };
        }
    }
}