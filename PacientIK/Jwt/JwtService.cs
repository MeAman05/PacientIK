using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using PacientIK.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace PacientIK.Jwt
{
    public class JwtService
    {
        private readonly IConfiguration cfg;

        public JwtService(IConfiguration cfg)
        {
            this.cfg = cfg;
        }

        public string GenerateToken(User user)
        {
            var tokenvalidity = cfg.GetValue<int>("JwtConfig:TokenValidityMins");
            var tokentime = DateTime.UtcNow.AddHours(tokenvalidity);

            var claimss = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("specId", user.SpecId.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var JwtToken = new JwtSecurityToken(
                expires: tokentime,
                claims: claimss,
                issuer: cfg["JwtConfig:Issuer"],
                audience: cfg["JwtConfig:Audience"],
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["JwtConfig:Key"])), SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(JwtToken);
        }
    }
}
