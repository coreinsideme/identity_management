using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using IdentityManagement.Entities;

namespace IdentityManagement.Services
{
    public class TokenService : ITokenService
    {
        public Jwt _jwt;

        public TokenService(IOptions<Jwt> jwt)
        {
            _jwt = jwt.Value;
        }

        public string GetToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwt.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role),
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwt.Audience,
                Issuer = _jwt.Issuer
            };

            foreach (var item in user.MethodsAllowed)
            {
                tokenDescriptor.Subject.AddClaim(new Claim("MethodAllowed", item));
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidAudience = _jwt.Audience,
                ValidIssuer = _jwt.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwt.Secret))
            };

            ClaimsPrincipal claims;
            try
            {
                claims = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
            }
            catch (Exception)
            {
                return false;
            }
            
            return claims.Claims.Any();
        }
    }
}
