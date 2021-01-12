using System.Collections.Generic;

using Moq;

using Xunit;

using TbspRpgLib.Entities;
using TbspRpgLib.Repositories;
using TbspRpgLib.Services;

namespace TbspRpgLib.Tests.Services {
    
    public class ServiceServiceTests {
        private IServiceService _serviceService;

        public ServiceServiceTests() {
            //we need to moq the service repository
            // var services = new List<Service>();
            // services.Add(new Service() {
            //     Id = "1",
            //     Name = "map",
            //     Url = "http://mapapi:8001"
            // });

            // var msrepo = new Mock<IServiceRepository>();
            // msrepo.Setup(repo => repo.GetServiceByName(It.IsAny<string>()))
            //     .Returns((string name) => services.Find(srv => srv.Name == name));
            // msrepo.Setup(repo => repo.GetAllServices()).Returns(services);

            _serviceService = new ServiceService(new ServiceRepository());
        }

        [Fact]
        public void GetServiceByName_Valid_ReturnResponse() {
            //arrange
            //act
            var service = _serviceService.GetServiceByName("map");

            //assert
            Assert.NotNull(service);
            Assert.Equal("94600c72-0d4c-4c9f-b247-56b366589534", service.Id.ToString());
        }

        [Fact]
        public void GetServiceByName_Invalid_NoResponse() {
            //arrange
            //act
            var service = _serviceService.GetServiceByName("pam");

            //assert
            Assert.Null(service);
        }

        [Fact]
        public void GetUrlForService_Valid_ReturnResponse() {
            //arrange
            //act
            var url = _serviceService.GetUrlForService("map");

            //assert
            Assert.NotNull(url);
            Assert.Equal("http://mapapi:8004/api", url);
        }

        [Fact]
        public void GetUrlForService_Invalid_NoResponse() {
            //arrange
            //act
            var url = _serviceService.GetUrlForService("pam");

            //assert
            Assert.Null(url);
        }

        [Fact]
        public void GetEventTypeByName_Valid_ReturnResponse() {
            //arrange
            //act
            var et = _serviceService.GetEventTypeByName("new_game");

            //assert
            Assert.NotNull(et);
            Assert.Equal("06072ad8-bf89-417a-8f9c-f8518ce16e70", et.Id.ToString());
        }

        [Fact]
        public void GetEventTypeByName_Invalid_NoResponse() {
            //arrange
            //act
            var url = _serviceService.GetEventTypeByName("new_gam");

            //assert
            Assert.Null(url);
        }
    }
}