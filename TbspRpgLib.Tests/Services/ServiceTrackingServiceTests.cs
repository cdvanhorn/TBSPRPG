using Moq;
using Xunit;
using TbspRpgLib.Entities;
using TbspRpgLib.Services;
using TbspRpgLib.Tests.Mocks;
using TbspRpgLib.Repositories;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TbspRpgLib.Tests.Services {
    public class ServiceTrackingServiceTests {
        private ServiceTrackingService _serviceTrackingService;
        private Service _service;
        private EventType _eventType;
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
            _eventId = Guid.NewGuid();

            _serviceTrackingService = ServiceTrackingServiceMock.MockServiceTrackingService();

            // eventTypePositions = new List<EventTypePosition>();
            // eventTypePositions.Add(new EventTypePosition() {
            //     Id = Guid.NewGuid(),
            //     ServiceId = _service.Id,
            //     EventTypeId = _eventType.Id,
            //     Position = 8
            // });

            
            // processedEvents = new List<ProcessedEvent>();
            // processedEvents.Add(new ProcessedEvent() {
            //     Id = Guid.NewGuid(),
            //     ServiceId = _service.Id,
            //     EventId = _eventId
            // });
            
            
        }

        [Fact]
        public async void GetStartPosition_ValidInput() {
            //arrange
            await _serviceTrackingService.UpdatePosition(
                _eventType.Id,
                8
            );

            //act
            var position = await _serviceTrackingService.GetPosition(
                _eventType.Id
            );

            //assert
            Assert.Equal<ulong>(8, position);
        }

        [Fact]
        public async void GetStartPosition_DoesntExist() {
            //act
            var position = await _serviceTrackingService.GetPosition(
                _eventType.Id
            );

            //assert
            Assert.Equal<ulong>(0, position);
        }

        //test update position
        [Fact]
        public async void UpdatePosition_NewPosition() {
            //arrange
            var etguid = Guid.NewGuid();

            //act
            await _serviceTrackingService.UpdatePosition(etguid, 42);

            //assert
            Assert.Equal<ulong>(42, 
                await _serviceTrackingService.GetPosition(etguid));
        }

        [Fact]
        public async void UpdatePosition_CurrentPosition() {
            //arrange
            await _serviceTrackingService.UpdatePosition(
                _eventType.Id,
                8
            );

            //act
            await _serviceTrackingService.UpdatePosition(_eventType.Id, 42);

            //assert
            Assert.Equal<ulong>(42, 
                await _serviceTrackingService.GetPosition(_eventType.Id));
        }

        [Fact]
        public async void UpdatePosition_OutdatedPosition() {
            //arrange
            await _serviceTrackingService.UpdatePosition(
                _eventType.Id,
                8
            );

            //act
            await _serviceTrackingService.UpdatePosition(_eventType.Id, 4);

            //assert
            Assert.Equal<ulong>(8, 
                await _serviceTrackingService.GetPosition(_eventType.Id));
        }

        //test has been processed
        [Fact]
        public async void HasBeenProcessed_Processed_ValidInput() {
            //arrange
            await _serviceTrackingService.EventProcessed(_eventId);

            //act
            bool processed = await _serviceTrackingService.HasBeenProcessed(_eventId);

            //assert
            Assert.True(processed);
        }

        [Fact]
        public async void HasBeenProcessed_NotProcessed_ValidInput() {
            //arrange, new event id
            Guid newEvent = Guid.NewGuid();

            //act
            bool processed = await _serviceTrackingService.HasBeenProcessed(newEvent);

            //assert
            Assert.False(processed);
        }

        //test event processed
        [Fact]
        public async void EventProcessed_NewEvent() {
            //arrange, new event
            Guid newEvent = Guid.NewGuid();

            //act
            await _serviceTrackingService.EventProcessed(newEvent);

            //assert
            Assert.True(await _serviceTrackingService.HasBeenProcessed(newEvent));
        }
    }
}