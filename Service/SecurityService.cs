using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.Models;

namespace Service
{
    public class SecurityService : ISecurityService
    {
        private readonly ConcurrentDictionary<string, string> _refreshTokens = new ConcurrentDictionary<string, string>();
        private readonly string _jwtSecret = "YourSuperSecretKeyHere"; // Replace with your actual secret
        private readonly int _jwtLifespan = 60; // JWT lifespan in minutes

        private readonly ISenProContext _context;


        public SecurityService(ISenProContext context)
        {
            _context = context;
        }

        public async Task<AuthResult> LoginAsync(string username, string password)
        {
            var user = await _context.UmUserAccounts.FirstOrDefaultAsync(u => u.UserId == username);
            if (user == null)
            {
                return await Task.FromResult(new AuthResult
                {
                    Success = false,
                    Errors = new List<string> { "Invalid username or password" }
                });
            }
            else
            {
                if (user.Password != password)
                {
                    return await Task.FromResult(new AuthResult
                    {
                        Success = false,
                        Errors = new List<string> { "Invalid username or password" }
                    });
                }
                else
                {
                    var token = GenerateJwtToken(username);
                    var refreshToken = GenerateRefreshToken();

                    _refreshTokens[refreshToken] = username;

                    return await Task.FromResult(new AuthResult
                    {
                        Token = token,
                        RefreshToken = refreshToken,
                        Success = true
                    });
                }
            }
        }

        public async Task LogoutAsync(string userId)
        {
            var tokensToRemove = _refreshTokens.Where(rt => rt.Value == userId).Select(rt => rt.Key).ToList();
            foreach (var token in tokensToRemove)
            {
                _refreshTokens.TryRemove(token, out _);
            }

            await Task.CompletedTask;
        }

        public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
        {
            if (_refreshTokens.TryGetValue(refreshToken, out var username))
            {
                var token = GenerateJwtToken(username);
                var newRefreshToken = GenerateRefreshToken();

                _refreshTokens[newRefreshToken] = username;
                _refreshTokens.TryRemove(refreshToken, out _);

                return await Task.FromResult(new AuthResult
                {
                    Token = token,
                    RefreshToken = newRefreshToken,
                    Success = true
                });
            }

            return await Task.FromResult(new AuthResult
            {
                Success = false,
                Errors = new List<string> { "Invalid refresh token" }
            });
        }

        public async Task ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            throw new Exception("Current password is incorrect");
            //if (_users.TryGetValue(userId, out var storedPassword) && storedPassword == currentPassword)
            //{
            //    _users[userId] = newPassword;
            //    await Task.CompletedTask;
            //}
            //else
            //{
            //    throw new Exception("Current password is incorrect");
            //}
        }

        public async Task InitiatePasswordResetAsync(string email)
        {
            // Simulate sending a password reset email
            await Task.CompletedTask;
        }

        public async Task ResetPasswordAsync(string userId, string token, string newPassword)
        {
            // In a real application, you would verify the token
            //if (_users.ContainsKey(userId))
            //{
            //    _users[userId] = newPassword;
            //    await Task.CompletedTask;
            //}
            //else
            //{
            throw new Exception("User not found");
            //}
        }

        public async Task<bool> IsUserAuthorizedAsync(string userId, string action)
        {
            // Implement your authorization logic here
            return await Task.FromResult(true);
        }

        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtLifespan),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
