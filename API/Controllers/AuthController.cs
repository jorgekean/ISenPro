using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.UserManagement;
using Service.UserManagement.Interface;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, IAuthService authService, ITokenService tokenService)
        {
            _authService = authService;
            _tokenService = tokenService;
            _logger = logger;
        }

        public class LoginRequest
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Validate user credentials.
                var user = await _authService.Authenticate(request.UserName, request.Password);
                if (user == null)
                {
                    return Unauthorized("Invalid credentials.");
                }

                // Generate JWT token.
                var accessToken = _tokenService.CreateToken(user);
                return Ok(new { accessToken, user });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        [Authorize]
        [HttpGet("accountInfo")]
        public async Task<IActionResult> GetAccountInfo()
        {
            try
            {
                // Retrieve the user identifier from the token claims.
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("User identifier not found in token.");
                }

                // Fetch user account information based on the userId.
                var accountInfo = await _authService.GetAccountInfo(int.Parse(userIdClaim));

                if (accountInfo == null)
                {
                    return NotFound("Account information not found.");
                }

                // Return the account info.
                return Ok(new { user = accountInfo });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
