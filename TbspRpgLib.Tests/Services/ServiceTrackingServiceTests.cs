using Moq;
using Xunit;
using TbspRpgLib.Entities;
using TbspRpgLib.Services;
using TbspRpgLib.Repositories;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TbspRpgLib.Tests.Services {
    public class ServiceTrackingServiceTests {
        private ServiceTrackingService _serviceTrackingService;
        private Service _service;
        private EventType _eventType;
        private bool _changesSaved;
        private List<EventTypePosition> eventTypePositions;
        private List<ProcessedEvent> processedEvents;
        private Guid _eventId;

        public ServiceTrackingServiceTests() {
            _service = new Service() {
                Id = Guid.NewGuid(),
                Name = "service"
            };
            _eventType = new EventType() {
                Id = Guid.NewGuid(),
                TypeName = "new_test"
            };

            eventTypePositions = new List<EventTypePosition>();
            eventTypePositions.Add(new EventTypePosition() {
                Id = Guid.NewGuid(),
                ServiceId = _service.Id,
                EventTypeId = _eventType.Id,
                Position = 8
            });

            _eventId = Guid.NewGuid();
            processedEvents = new List<ProcessedEvent>();
            processedEvents.Add(new ProcessedEvent() {
                Id = Guid.NewGuid(),
                ServiceId = _service.Id,
                EventId = _eventId
            });
            
            var mstrepo = new Mock<IServiceTrackingRepository>();
            //get an event type position
            mstrepo.Setup(repo => repo.GetEventTypePosition(It.IsAny<Guid>(), It.IsAny<Guid>()))
                 .ReturnsAsync((Guid ser, Guid et) => eventTypePositions.Where(
                     etp => etp.ServiceId == ser && etp.EventTypeId == et).FirstOrDefault()
                );
                
            //insert event type position
            mstrepo.Setup(repo =>
                repo.InsertEventTypePosition(It.IsAny<EventTypePosition>())
            ).Callback<EventTypePosition>((etp) => eventTypePositions.Add(etp));

            //save changes
            _changesSaved = false;
            mstrepo.Setup(repo =>
                repo.SaveChanges()
            ).Callback(() => _changesSaved = true);

            //get processed event
            mstrepo.Setup(repo => repo.GetProcessedEvent(It.IsAny<Guid>(), It.IsAny<Guid>()))
                 .ReturnsAsync((Guid ser, Guid et) => processedEvents.Where(
                     etp => etp.ServiceId == ser && etp.EventId == et).FirstOrDefault()
                );

            //event processed
            mstrepo.Setup(repo =>
                repo.InsertProcessedEvent(It.IsAny<ProcessedEvent>())
            ).Callback<ProcessedEvent>((pe) => processedEvents.Add(pe));

            // msrepo.Setup(repo => repo.GetAllServices()).ReturnsAsync(services);
            _serviceTrackingService = new ServiceTrackingService(mstrepo.Object);
        }

        [Fact]
        public async void GetStartPosition_ValidInput() {
            //arrange
            //act
            var position = await _serviceTrackingService.GetPosition(
                _service.Id,
                _eventType.Id
            );

            //assert
            Assert.Equal<ulong>(8, position);
        }

        [Fact]
        public async void GetStartPosition_DoesntExist() {
            //act
            var sguid = Guid.NewGuid();
            var position = await _serviceTrackingService.GetPosition(
                sguid,
                _eventType.Id
            );

            //assert
            Assert.Equal<ulong>(0, position);
        }

        //test update position
        [Fact]
        public void UpdatePosition_NewPosition() {
            //arrange
            var sguid = Guid.NewGuid();
            var etguid = Guid.NewGuid();

            //act
            _serviceTrackingService.UpdatePosition(sguid, etguid, 42);

            //assert
            Assert.Equal(2, eventTypePositions.Count());
            Assert.Equal<ulong>(42, 
                eventTypePositions.Where(etp => etp.ServiceId == sguid).First().Position);
        }

        [Fact]
        public void UpdatePosition_CurrentPosition() {
            //arrange
            //act
            _serviceTrackingService.UpdatePosition(_service.Id, _eventType.Id, 42);

            //assert
            Assert.Single(eventTypePositions);
            Assert.Equal<ulong>(42, 
                eventTypePositions.First().Position);
            Assert.True(_changesSaved);
        }

        [Fact]
        public void UpdatePosition_OutdatedPosition() {
            //arrange
            //act
            _serviceTrackingService.UpdatePosition(_service.Id, _eventType.Id, 4);

            //assert
            Assert.Single(eventTypePositions);
            Assert.Equal<ulong>(8, 
                eventTypePositions.First().Position);
            Assert.False(_changesSaved);
        }

        //test has been processed
        [Fact]
        public async void HasBeenProcessed_Processed_ValidInput() {
            //arrange
            //act
            bool processed = await _serviceTrackingService.HasBeenProcessed(_service.Id, _eventId);

            //assert
            Assert.True(processed);
        }

        [Fact]
        public async void HasBeenProcessed_NotProcessed_ValidInput() {
            //arrange, new event id
            Guid newEvent = Guid.NewGuid();

            //act
            bool processed = await _serviceTrackingService.HasBeenProcessed(_service.Id, newEvent);

            //assert
            Assert.False(processed);
        }

        //test event processed
        [Fact]
        public async void EventProcessed_NewEvent() {
            //arrange, new event
            Guid newEvent = Guid.NewGuid();

            //act
            _serviceTrackingService.EventProcessed(_service.Id, newEvent);

            //assert
            Assert.Equal(2, processedEvents.Count);
            Assert.True(await _serviceTrackingService.HasBeenProcessed(_service.Id, newEvent));
        }
    }
}