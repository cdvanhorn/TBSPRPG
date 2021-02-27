using System.Collections.Generic;
using System.Linq;

using TbspRpgLib.Entities;
using TbspRpgLib.Aggregates;

namespace TbspRpgLib.Repositories {
    public interface IAggregateTypeRepository {
        AggregateType GetAggregateTypeByType(string typeName);
        AggregateType GetAggregateTypeByPrefix(string prefix);
        List<AggregateType> GetAll();
    }

    public class AggregateTypeRepository : IAggregateTypeRepository{
        private List<AggregateType> _aggregateTypes;

        public const string CONTENT_AGGREGATE_PREFIX = "content";
        public const string CONTENT_AGGREGATE_TYPE = "ContentAggregate";
        public const string GAME_AGGREGATE_TYPE = "GameAggregate";
        public const string DEFAULT_AGGREGATE_TYPE = GAME_AGGREGATE_TYPE;
        public const char AGGREGATE_ID_DIVIDER = '_';

        public AggregateTypeRepository() {
            _aggregateTypes = new List<AggregateType>();

            _aggregateTypes.Add(new AggregateType {
                TypeName = GAME_AGGREGATE_TYPE,
                AggregateIdPrefix = null
            });

            _aggregateTypes.Add(new AggregateType {
                TypeName = CONTENT_AGGREGATE_TYPE,
                AggregateIdPrefix = CONTENT_AGGREGATE_PREFIX
            });
        }

        public AggregateType GetAggregateTypeByType(string typeName) {
            return _aggregateTypes.Where(at => at.TypeName == typeName).FirstOrDefault();
        }

        public AggregateType GetAggregateTypeByPrefix(string prefix) {
            return _aggregateTypes.Where(at => at.AggregateIdPrefix == prefix).FirstOrDefault();
        }

        public List<AggregateType> GetAll() {
            return _aggregateTypes;
        }
    }
}