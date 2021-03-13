using System.Threading.Tasks;

namespace TbspRpgLib.InterServiceCommunication {
    public interface IAdventureServiceLink {
        Task<IscResponse> GetInitialLocation(string adventureId, string userId);
    }

    public class AdventureServiceLink : BaseServiceLink, IAdventureServiceLink {
        public AdventureServiceLink(IServiceCommunication serviceCommuncation) : base(serviceCommuncation) {
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