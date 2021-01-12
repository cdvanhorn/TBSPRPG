using System.Collections.Generic;
using System.Linq;
using System;

using TbspRpgLib.Entities;

using Xunit;

namespace TbspRpgLib.Tests.Entities {
    public class ServiceTests {
        // //we need to mock the service tracking repository
        // [Fact]
        // public void GetStartPosition_Indexes() {
        //     //arrange
        //     Service service = new Service();
        //     service.Id = "1";
        //     service.Name = "test";
            
        //     //act
        //     var position = service.GetStartPosition("new_game");

        //     //assert
        //     Assert.Equal<ulong>(8, position);
        // }

        // [Fact]
        // public void UpdatePosition_Indexes() {
        //     //arrange
        //     Service service = new Service();
        //     service.Id = "1";
        //     service.Name = "test";
            
        //     //act
        //     var shouldUpdate = service.UpdatePosition(42, "new_game");

        //     //assert
        //     Assert.True(shouldUpdate);
        //     // Assert.Single(service.EventIndexes);
        //     // Assert.Equal<ulong>(42, service.EventIndexes.First().Index);
        //     // Assert.Equal("new_game", service.EventIndexes.First().EventName);
        // }

        // [Fact]
        // public void UpdatePosition_PreviousPosition() {
        //     //arrange
        //     Service service = new Service();
        //     service.Id = "1";
        //     service.Name = "test";
            
        //     //act
        //     var shouldUpdate = service.UpdatePosition(42, "new_game");

        //     //assert
        //     Assert.False(shouldUpdate);
        //     Assert.Single(service.EventIndexes);
        //     // Assert.Equal<ulong>(45, service.EventIndexes.First().Index);
        //     // Assert.Equal("new_game", service.EventIndexes.First().EventName);
        // }
    }
}