using Authentication.Core.Dtos;
using Authentication.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Infrastructure.Jwt
{
    public class JwtManager : IJwt
    {
        private readonly ILogger<JwtManager> _log;
        private readonly JwtConfiguration _jwtConfiguration;

        public JwtManager(IOptions<JwtConfiguration> jwt, ILogger<JwtManager> log)
        {
            _jwtConfiguration = jwt.Value;
            _log = log;
        }

        public string GenerateToken(string user_id, string user_email, List<string> roles, int expiration_time = 60)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Email, user_email),
                    new Claim(ClaimTypes.NameIdentifier, user_id),
                    new Claim(ClaimTypes.Name, user_id)
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddMinutes(expiration_time),
                    SigningCredentials = credentials,
                    Audience = _jwtConfiguration.Audience,
                    Issuer = _jwtConfiguration.Issuer,
                    IssuedAt = DateTime.UtcNow
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return null;
            }

        }

        public async Task<bool> ValidateToken(string token, string secret)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = Encoding.UTF8.GetBytes(secret);

                var validationSettings = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };

                tokenHandler.ValidateToken(token, validationSettings, out var securityToken);

                var jwtToken = securityToken;

                return true;
            }
            catch (Exception ex)
            {
                _log.LogError(GetType().FullName + Environment.NewLine + MethodBase.GetCurrentMethod().ReflectedType.Name + Environment.NewLine + ex.Message);
                return false;
            }

        }
    }
}
