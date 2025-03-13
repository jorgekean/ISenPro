using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using EF.Models;
    using global::Service.Dto.UserManagement;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
   

    public interface ITokenService
    {
        string CreateToken(UserAccountAuthModel user);
    }

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(UserAccountAuthModel user)
        {
            // Create claims for the token. You can add additional claims if needed.
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserAccountId.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Role, user.RoleCode.ToString()),
            // add more custom clains
            new Claim("Email", string.IsNullOrWhiteSpace(user.Email) ? "No Email" : user.Email),
            new Claim("Department", user.Department),
            new Claim("RoleName", user.RoleName)
        };

            // Retrieve JWT settings from configuration.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"]);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(expireMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
