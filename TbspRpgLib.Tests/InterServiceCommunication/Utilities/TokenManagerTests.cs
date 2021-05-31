using System;
using TbspRpgLib.InterServiceCommunication.Utilities;
using TbspRpgLib.Jwt;
using TbspRpgLib.Settings;
using Xunit;

namespace TbspRpgLib.Tests.InterServiceCommunication.Utilities
{
    public class TokenManagerTests
    {
        #region Setup

        private Guid _testUserId = Guid.NewGuid();
        private readonly string _testToken = Guid.NewGuid().ToString();

        private static TokenManager CreateTokenManager()
        {
            return new TokenManager(new JwtSettings()
            {
                Secret = Guid.NewGuid().ToString()
            });
        }

        private void PopulateToken(ITokenManager manager)
        {
            manager.AddTokenForUserId(_testUserId.ToString(), _testToken);
        }

        #endregion
        
        #region GetTokenForUserId

        [Fact]
        public void GetTokenForUserId_Exists_ReturnsToken()
        {
            //arrange
            var manager = CreateTokenManager();
            PopulateToken(manager);
            
            //act
            var token = manager.GetTokenForUserId(_testUserId.ToString());
            
            //assert
            Assert.Equal(_testToken, token);
        }

        [Fact]
        public void GetTokenForUserId_NotExists_GeneratesToken()
        {
            //arrange
            var manager = CreateTokenManager();
            PopulateToken(manager);
            
            //act
            var token = manager.GetTokenForUserId(Guid.NewGuid().ToString());
            
            //assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        #endregion
        
        #region AddTokenForUserId

        [Fact]
        public void AddTokenForUserId_Exists_OverwriteToken()
        {
            //arrange
            var manager = CreateTokenManager();
            PopulateToken(manager);
            
            //act
            manager.AddTokenForUserId(_testUserId.ToString(), "token_banana");
            var token = manager.GetTokenForUserId(_testUserId.ToString());
            
            //assert
            Assert.NotEqual(_testToken, token);
            Assert.Equal("token_banana", token);
        }

        [Fact]
        public void AddTokenForUserId_NotExist_TokenAdded()
        {
            //arrange
            var manager = CreateTokenManager();
            PopulateToken(manager);
            var newUserId = Guid.NewGuid().ToString();
            
            //act
            manager.AddTokenForUserId(newUserId, "token_banana");
            var token = manager.GetTokenForUserId(newUserId);
            
            //assert
            Assert.Equal("token_banana", token);
        }
        
        #endregion

        #region MultipleManagers

        [Fact]
        public void MultipleManagers_AccessSameToken()
        {
            //arrange
            var manager = CreateTokenManager();
            PopulateToken(manager);
            var manager2 = CreateTokenManager();
            var newUserId = Guid.NewGuid().ToString();
            
            //act
            var genToken = manager.GetTokenForUserId(newUserId);
            var token = manager2.GetTokenForUserId(newUserId);
            
            //assert
            Assert.Equal(genToken, token);
        }

        #endregion
    }
}