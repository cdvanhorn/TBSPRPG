using TbspRpgLib.Settings;
using TbspRpgLib.Entities;
using System.Collections.Generic;

namespace TbspRpgLib.EventProcessors {
    public abstract class MultiEventProcessor : EventProcessorService {
        
        public MultiEventProcessor(string serviceName, IEnumerable<string> eventTypeNames, IEventStoreSettings eventStoreSettings) : 
                base(serviceName, eventStoreSettings) {      
            foreach(var eventTypeName in eventTypeNames) {
                _eventTypes.Add(
                    _serviceService.GetEventTypeByName(eventTypeName)
                );
            }
        }

        protected EventType GetEventTypeByName(string eventTypeName) {
            return _serviceService.GetEventTypeByName(eventTypeName);
        }
    }
}