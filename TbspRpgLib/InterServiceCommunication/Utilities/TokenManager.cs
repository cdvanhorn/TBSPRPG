using System;
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
        private readonly Dictionary<string, string> _tokens;
        private readonly IJwtHelper _jwtHelper;

        public TokenManager(IJwtSettings jwtSettings)
        {
            _tokens = new Dictionary<string, string>();
            _jwtHelper = new JwtHelper(jwtSettings.Secret);
        }
        
        public string GetTokenForUserId(string userId)
        {
            if(!_tokens.ContainsKey(userId)) {
                _tokens.Add(userId, _jwtHelper.GenerateToken(userId));
            }
            return _tokens[userId];
        }

        public void AddTokenForUserId(string userId, string token)
        {
            if(!_tokens.ContainsKey(userId))
                _tokens.Add(userId, token);
            else
                _tokens[userId] = token;
        }
    }
}