using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BTS.App.Data.Repository.Interface;
using Microsoft.IdentityModel.Tokens;
using NMS.Assistant.Domain.Configuration.Interface;
using NMS.Assistant.Domain.Constants;
using NMS.Assistant.Domain.Contract;
using NMS.Assistant.Domain.Result;

namespace NMS.Assistant.Data.Repository
{
    public class JwtRepository : IJwtRepository
    {
        private readonly IJwt _jwtConfig;

        public JwtRepository(IJwt jwtConfig)
        {
            _jwtConfig = jwtConfig;
        }

        public ResultWithValue<string> GenerateToken(User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(CustomClaimTypes.UserId, user.Guid.ToString()),
                    new Claim(ClaimTypes.Expiration, _jwtConfig.TimeValidInSeconds.ToString()), 
                    new Claim(ClaimTypes.AuthenticationMethod, "JWT"),
                }),
                Expires = DateTime.Now.AddSeconds(_jwtConfig.TimeValidInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                IssuedAt = DateTime.Now,
                Audience = _jwtConfig.Audience,
                Issuer = _jwtConfig.Issuer,  
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return new ResultWithValue<string>(true, tokenHandler.WriteToken(token), string.Empty);
        }
    }
}
