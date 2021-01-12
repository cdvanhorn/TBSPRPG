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

        public ServiceTrackingServiceTests() {
            //need to moq the ServiceTrackingRepository
            // var services = new List<Service>();
            // services.Add(new Service() {
            //     Id = "1",
            //     Name = "map",
            //     Url = "http://mapapi:8001"
            // });
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
        //test event processed
    }
}