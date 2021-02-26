using TbspRpgLib.Repositories;
using TbspRpgLib.Entities;

using System.Collections.Generic;

namespace TbspRpgLib.Services {
    public interface IAggregateTypeService {
        string GetAggregateTypeName(string aggregateId);
        string GetPrefixForAggregateType(string aggregateTypeName);
        string GenerateAggregateIdForAggregateType(string idWithoutPrefix, string aggregateTypeName);
        List<AggregateType> GetAggregateTypes();
    }

    public class AggregateTypeService : IAggregateTypeService{
        private IAggregateTypeRepository _repository;

        public AggregateTypeService(IAggregateTypeRepository repository) {
            _repository = repository;
        }

        public string GetAggregateTypeName(string aggregateId) {
            return null;
        }

        public string GetPrefixForAggregateType(string aggregateTypeName) {
            return null;
        }

        public string GenerateAggregateIdForAggregateType(string idWithoutPrefix, string aggregateTypeName) {
            return null;
        }

        public List<AggregateType> GetAggregateTypes() {
            return null;
        }
    }
}