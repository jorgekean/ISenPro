using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    // UserContext service
    public interface IUserContext
    {
        int UserId { get; }
        string UserName { get; }
        string RoleCode { get; }
        bool IsAdmin { get; }
    }

    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int UserId => Convert.ToInt32(_httpContextAccessor.HttpContext?.User?
            .FindFirst(ClaimTypes.NameIdentifier)?.Value);

        public string UserName => _httpContextAccessor.HttpContext?.User?
            .Identity?.Name ?? "";

        public string RoleCode => _httpContextAccessor.HttpContext?.User?
           .FindFirst(ClaimTypes.Role)?.Value ?? "";

        // convert to bool
        public bool IsAdmin => _httpContextAccessor.HttpContext?.User?
            .FindFirst("IsAdmin")?.Value == "True" ? true : false;
    }
}
