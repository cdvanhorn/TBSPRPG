using AdventureApi.Entities;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AdventureApi.Repositories;

namespace AdventureApi.Services {
    public interface IAdventuresService {
        Task<List<Adventure>> GetAll();
    }

    public class AdventuresService : IAdventuresService {
        private IAdventuresRepository _adventuresRespository;

        public AdventuresService(IAdventuresRepository adventuresRepository) {
            _adventuresRespository = adventuresRepository;
        }
        
        public Task<List<Adventure>> GetAll() {
            return _adventuresRespository.GetAllAdventures();
        }
    }
}