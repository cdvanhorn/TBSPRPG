using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TbspRpgLib.Jwt;
using TbspRpgLib.Settings;

namespace TbspRpgLib.InterServiceCommunication.Utilities
{
    public interface ITokenManager
    {
        string GetTokenForUserId(string userId);
        void AddTokenForUserId(string userId, string token);
    }
    
    public class TokenManager : ITokenManager
    {
        private static readonly ConcurrentDictionary<string, string> _tokens
            = new ConcurrentDictionary<string, string>();
        private readonly IJwtHelper _jwtHelper;

        public TokenManager(IJwtSettings jwtSettings)
        {
            _jwtHelper = new JwtHelper(jwtSettings.Secret);
        }
        
        public string GetTokenForUserId(string userId)
        {
            if(!_tokens.ContainsKey(userId))
            {
                _tokens[userId] = _jwtHelper.GenerateToken(userId);
            }
            return _tokens[userId];
        }

        public void AddTokenForUserId(string userId, string token)
        {
            _tokens[userId] = token;
        }
    }
}