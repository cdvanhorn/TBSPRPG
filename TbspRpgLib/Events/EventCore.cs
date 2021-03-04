using TbspRpgLib.Repositories;

namespace TbspRpgLib.Events {
    public abstract class EventCore : Event {
        protected EventContent Data { get; set; }

        protected override EventContent GetData() {
            return Data;
        }

        // public override string GetStreamId() {
        //     if(string.IsNullOrEmpty(GetStreamIdPrefix()))
        //         return Data.Id;
        //     return $"{GetStreamIdPrefix()}{AggregateTypeRepository.AGGREGATE_ID_DIVIDER}{Data.Id}";
        // }

        // public override string GetStreamIdPrefix()
        // {
        //     return "";
        // }

        public override string GetDataId()
        {
            return Data.Id;
        }

        public override string ToString() {
            return $"{EventId}\n{Type}\n{GetDataJson()}";
        }
    }
}