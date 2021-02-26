using System.Collections.Generic;

using TbspRpgLib.Entities;
using TbspRpgLib.Aggregates;

namespace TbspRpgLib.Repositories {
    public interface IAggregateTypeRepository {

    }

    public class AggregateTypeRepository : IAggregateTypeRepository{
        private List<AggregateType> _aggregateTypes;

        public const string CONTENT_AGGREGATE_PREFIX = "content";
        public const string CONTENT_AGGREGATE_TYPE = "ContentAggregate";
        public const string GAME_AGGREGATE_TYPE = "GameAggregate";

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
    }
}