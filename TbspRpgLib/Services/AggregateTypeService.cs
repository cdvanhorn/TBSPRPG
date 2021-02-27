using TbspRpgLib.Repositories;
using TbspRpgLib.Entities;

using System;
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
            string[] id_parts = aggregateId.Split(AggregateTypeRepository.AGGREGATE_ID_DIVIDER);
            if(id_parts.Length > 1) {
                var aggType = _repository.GetAggregateTypeByPrefix(id_parts[0]);
                if(aggType != null)
                    return aggType.TypeName;
                else
                    throw new ArgumentException($"invalid aggregate id {aggregateId}");
            }
            return AggregateTypeRepository.DEFAULT_AGGREGATE_TYPE;
        }

        public string GetPrefixForAggregateType(string aggregateTypeName) {
            var aggType = _repository.GetAggregateTypeByType(aggregateTypeName);
            if(aggType == null)
                throw new ArgumentException($"invalid aggregate type {aggregateTypeName}");
            return aggType.AggregateIdPrefix;
        }

        public string GenerateAggregateIdForAggregateType(string idWithoutPrefix, string aggregateTypeName) {
            var prefix = GetPrefixForAggregateType(aggregateTypeName);
            return $"{prefix}{AggregateTypeRepository.AGGREGATE_ID_DIVIDER}{idWithoutPrefix}";
        }

        public List<AggregateType> GetAggregateTypes() {
            return _repository.GetAll();
        }
    }
}