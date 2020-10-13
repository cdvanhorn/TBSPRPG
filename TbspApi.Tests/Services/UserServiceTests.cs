using Moq;

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

using TbspApi.Entities;
using TbspApi.Models;
using TbspApi.Services;
using TbspApi.Repositories;
using TbspApi.Utilities;

using Xunit;

namespace TbspApi.Tests.Services {
    public class UserServiceTests {
        private readonly UserService _userService;

        public UserServiceTests() {
            //need to mock database settings and jwtsettings
            var mjwt = new Mock<IJwtSettings>();
            mjwt.Setup(jwt => jwt.Secret).Returns("SUPER DUPER SECRET");
            var mdb = new Mock<IDatabaseSettings>();
            mdb.Setup(db => db.Salt).Returns("y728sfLla98YUZpTgCM4VA==");

            //mock user repository
            var users = new List<User>();
            users.Add(new User() {
                Id = "8675309",
                Username = "test",
                Password = "g4XyaMMxqIwlm0gklTRldD3PrM/xYTDWmpvfyKc8Gi4=" //hashed version of "test"
            });
            var murepo = new Mock<IUserRepository>();
            murepo.Setup(repo => repo.GetUserByUsernameAndPassword(
                It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(
                    (string u, string p) => users.Find(usr => usr.Username == u && usr.Password == p));

            _userService = new UserService(mjwt.Object, mdb.Object, murepo.Object);
        }

        [Fact]
        public async void Authenticate_IsValid_ReturnResponse() {
            //arrange
            AuthenticateRequest req = new AuthenticateRequest() {
                Username = "test",
                Password = "test"
            };

            //act
            var response = await _userService.Authenticate(req);

            //assert
            Assert.NotNull(response);
            Assert.NotEmpty(response.Token);
            Assert.Equal("test", response.Username);
            Assert.Equal("8675309", response.Id);
        }

        [Fact]
        public async void Authenticate_InvalidPassword_ReturnNull() {
            //arrange
            AuthenticateRequest req = new AuthenticateRequest() {
                Username = "test",
                Password = "testt"
            };

            //act
            var response = await _userService.Authenticate(req);

            //assert
            Assert.Null(response);
        }

        [Fact]
        public async void Authenticate_InvalidUsername_ReturnNull() {
            //arrange
            AuthenticateRequest req = new AuthenticateRequest() {
                Username = "testt",
                Password = "test"
            };

            //act
            var response = await _userService.Authenticate(req);

            //assert
            Assert.Null(response);
        }
    }

    
}