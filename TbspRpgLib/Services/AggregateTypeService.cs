using TbspRpgLib.Repositories;
using TbspRpgLib.Entities;

using System;
using System.Collections.Generic;

namespace TbspRpgLib.Services {
    public interface IAggregateTypeService {
        string GetAggregateTypeName(string aggregateId);
        string GetPrefixForAggregateType(string aggregateTypeName);
        string GenerateAggregateIdForAggregateType(string idWithoutPrefix, string aggregateTypeName);
        bool IsIdAlreadyPrefixed(string aggregateId, string aggregateTypeName);
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
            if(prefix != null)
                return $"{prefix}{AggregateTypeRepository.AGGREGATE_ID_DIVIDER}{idWithoutPrefix}";
            else
                return idWithoutPrefix;
        }

        public bool IsIdAlreadyPrefixed(string aggregateId, string aggregateTypeName) {
            var prefix = GetPrefixForAggregateType(aggregateTypeName);
            string[] id_parts = aggregateId.Split(AggregateTypeRepository.AGGREGATE_ID_DIVIDER);
            bool idHasPrefix = id_parts.Length > 1 ? true : false;
            if(prefix == null && !idHasPrefix)
                return true;
            else if(prefix != null && !idHasPrefix)
                return false;  //there is a prefix but it hasn't been applied to the id
            else if(prefix != null && idHasPrefix) {
                //if the prefixes don't match there's a big problem
                if(prefix == id_parts[0])
                    return true;  //the id has a prefix and it matches what it should be
                else
                    throw new Exception(
                        $"{aggregateId} of type {aggregateTypeName} has the wrong prefix, prefix is {id_parts[0]} should be {prefix}"
                    );
            }
            else if(prefix == null && idHasPrefix)
                throw new Exception(
                    $"{aggregateId} of type {aggregateTypeName} has prefix when it should not"
                );
            else  //probably should never get here
                throw new Exception(
                    $"{aggregateId} of type {aggregateTypeName} can't determine if has prefix or not"
                );
        }

        public List<AggregateType> GetAggregateTypes() {
            return _repository.GetAll();
        }
    }
}