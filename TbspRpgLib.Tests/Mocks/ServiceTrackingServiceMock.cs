using TbspRpgLib.Services;
using TbspRpgLib.Repositories;
using TbspRpgLib.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TbspRpgLib.Tests.Mocks {
    public class ServiceTrackingServiceMock {
        public static ServiceTrackingService MockServiceTrackingService() {
            List<EventTypePosition> eventTypePositions = new List<EventTypePosition>();
            List<ProcessedEvent> processedEvents = new List<ProcessedEvent>();

            var mstrepo = new Mock<IServiceTrackingRepository>();
            //get an event type position
            mstrepo.Setup(repo => repo.GetEventTypePosition(It.IsAny<Guid>()))
                 .ReturnsAsync((Guid et) => eventTypePositions.Where(
                     etp => etp.EventTypeId == et).FirstOrDefault()
                );
                
            //insert event type position
            mstrepo.Setup(repo =>
                repo.AddEventTypePosition(It.IsAny<EventTypePosition>())
            ).Callback<EventTypePosition>((etp) => eventTypePositions.Add(etp));

            //save changes
            // mstrepo.Setup(repo =>
            //     repo.SaveChanges()
            // ).Callback(() => _ = true);

            //get processed event
            mstrepo.Setup(repo => repo.GetProcessedEvent(It.IsAny<Guid>()))
                 .ReturnsAsync((Guid et) => processedEvents.Where(
                     etp => etp.EventId == et).FirstOrDefault()
                );

            //event processed
            mstrepo.Setup(repo =>
                repo.AddProcessedEvent(It.IsAny<ProcessedEvent>())
            ).Callback<ProcessedEvent>((pe) => processedEvents.Add(pe));

            // msrepo.Setup(repo => repo.GetAllServices()).ReturnsAsync(services);
            return new ServiceTrackingService(mstrepo.Object);
        }
    }
}