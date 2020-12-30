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
            var services = new List<Service>();
            services.Add(new Service() {
                Id = "1",
                Name = "map",
                Url = "http://mapapi:8001",
                EventPrefix = "1"
            });

            var msrepo = new Mock<IServiceRepository>();
            msrepo.Setup(repo => repo.GetServiceByName(It.IsAny<string>()))
                .ReturnsAsync((string name) => services.Find(srv => srv.Name == name));
            msrepo.Setup(repo => repo.GetAllServices()).ReturnsAsync(services);

            _serviceService = new ServiceService(msrepo.Object);
        }

        [Fact]
        public async void GetServiceByName_Valid_ReturnResponse() {
            //arrange
            //act
            var service = await _serviceService.GetServiceByName("map");

            //assert
            Assert.NotNull(service);
            Assert.Equal("1", service.Id);
        }

        [Fact]
        public async void GetServiceByName_Invalid_NoResponse() {
            //arrange
            //act
            var service = await _serviceService.GetServiceByName("pam");

            //assert
            Assert.Null(service);
        }
    }
}