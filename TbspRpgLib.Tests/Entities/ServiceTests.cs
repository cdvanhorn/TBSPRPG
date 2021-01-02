using System.Collections.Generic;
using System.Linq;
using System;

using TbspRpgLib.Entities;

using Xunit;

namespace TbspRpgLib.Tests.Entities {
    public class ServiceTests {
        [Fact]
        public void GetStartPosition_NoIndexes() {
            //arrange
            Service service = new Service();
            service.Id = "1";
            service.Name = "test";
            
            //act and assert
            Assert.Throws<InvalidOperationException>(() => service.GetStartPosition("new_game"));
        }

        [Fact]
        public void GetStartPosition_EmptyIndexes() {
            //arrange
            Service service = new Service();
            service.Id = "1";
            service.Name = "test";
            service.EventIndexes = new List<EventIndex>();
            
            //act and assert
            Assert.Throws<InvalidOperationException>(() => service.GetStartPosition("new_game"));
        }

        [Fact]
        public void GetStartPosition_Indexes() {
            //arrange
            Service service = new Service();
            service.Id = "1";
            service.Name = "test";
            service.EventIndexes = new List<EventIndex>();
            service.EventIndexes.Add(new EventIndex() {
                EventName = "new_game",
                Index = 8
            });
            
            //act
            var position = service.GetStartPosition("new_game");

            //assert
            Assert.Equal<ulong>(8, position);
        }

        [Fact]
        public void UpdatePostion_NoIndexes() {
            //arrange
            Service service = new Service();
            service.Id = "1";
            service.Name = "test";

            //act and assert
            Assert.Throws<InvalidOperationException>(() => service.UpdatePosition(42, "new_game"));
        }

        [Fact]
        public void UpdatePostion_EmptyIndexes() {
            //arrange
            Service service = new Service();
            service.Id = "1";
            service.Name = "test";
            service.EventIndexes = new List<EventIndex>();
            
            //act and assert
            Assert.Throws<InvalidOperationException>(() => service.UpdatePosition(42, "new_game"));
        }

        [Fact]
        public void UpdatePosition_Indexes() {
            //arrange
            Service service = new Service();
            service.Id = "1";
            service.Name = "test";
            service.EventIndexes = new List<EventIndex>();
            service.EventIndexes.Add(new EventIndex() {
                EventName = "new_game",
                Index = 8
            });
            
            //act
            var shouldUpdate = service.UpdatePosition(42, "new_game");

            //assert
            Assert.True(shouldUpdate);
            Assert.Single(service.EventIndexes);
            Assert.Equal<ulong>(42, service.EventIndexes.First().Index);
            Assert.Equal("new_game", service.EventIndexes.First().EventName);
        }

        [Fact]
        public void UpdatePosition_PreviousPosition() {
            //arrange
            Service service = new Service();
            service.Id = "1";
            service.Name = "test";
            service.EventIndexes = new List<EventIndex>();
            service.EventIndexes.Add(new EventIndex() {
                EventName = "new_game",
                Index = 45
            });
            
            //act
            var shouldUpdate = service.UpdatePosition(42, "new_game");

            //assert
            Assert.False(shouldUpdate);
            Assert.Single(service.EventIndexes);
            Assert.Equal<ulong>(45, service.EventIndexes.First().Index);
            Assert.Equal("new_game", service.EventIndexes.First().EventName);
        }
    }
}