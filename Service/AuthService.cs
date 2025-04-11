using EF.Models;
using Microsoft.EntityFrameworkCore;

namespace Service
{

    public class UserAccountAuthModel
    {
        public int UserAccountId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string Department { get; set; }
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }

        public List<string> ModuleNames { get; set; }


        // rolebasedbutton fields
        public bool IsAdmin { get; set; }
        public bool CanPrint { get; set; }
        public bool CanApprove { get; set; }

    }

    public interface IAuthService
    {
        Task<UserAccountAuthModel> Authenticate(string userName, string password);
        Task<UserAccountAuthModel> GetAccountInfo(int id);
    }

    public class AuthService : IAuthService
    {
        private readonly ISenProContext _context;
        private string _userName = string.Empty;
        public AuthService(ISenProContext context)
        {
            _context = context;
        }

        public async Task<UserAccountAuthModel> Authenticate(string userName, string password)
        {
            _userName = userName;

            // Retrieve the user by username
            var user = await _context.UmUserAccounts.SingleOrDefaultAsync(u => u.UserId == userName);
            if (user == null)
                return null;

            // Validate password using your hashing mechanism
            // For demonstration, assume a ValidatePassword method exists:
            if (!ValidatePassword(password, user.Password))
                return null;


            var authUser = await GetAccountInfo(user.UserAccountId);

            return authUser;
        }

        // Get Account Info
        public async Task<UserAccountAuthModel> GetAccountInfo(int id)
        {
            UserAccountAuthModel? result = null;
            var userInfo = await _context.UmUserAccounts
                .Include(s => s.Person).ThenInclude(t => t.Department)
                .Include(s => s.Role)
                .FirstOrDefaultAsync(f => f.UserAccountId == id);

            if (userInfo != null)
            {
                // distinct module names
                var moduleNames = _context.VRoleModuleControls
               .Where(x => x.RoleId == userInfo.RoleId.Value)
               .GroupBy(x => new { x.RoleId, x.ParentModuleName, x.ModuleName })
               .Select(s => s.Key.ModuleName.ToLower() ?? "").ToList();

                result = new UserAccountAuthModel
                {
                    UserAccountId = userInfo.UserAccountId,
                    UserName = userInfo.UserId,
                    DisplayName = $"{userInfo.Person.LastName}, {userInfo.Person.FirstName}",
                    Email = userInfo.Person?.Email,
                    RoleCode = userInfo.Role?.Code,
                    RoleName = userInfo.Role?.Name,
                    Department = userInfo.Person?.Department?.Name,
                    DepartmentId = userInfo.Person?.DepartmentId ?? 0,
                    RoleId = userInfo.RoleId ?? 0,
                    IsAdmin = userInfo.IsAdmin,

                    ModuleNames = moduleNames
                };
            }

            return result;
        }

        private bool ValidatePassword(string password, string storedHash)
        {
            // use old code for password hashing, after all users login we can remove the old checking code
            if (OldAuthentication(password, ref storedHash))
            {
                return true;
            }

            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

        private bool OldAuthentication(string password, ref string storedHash)
        {
            if (OldPasswordHashing.ValidatePassword(password, storedHash))
            {
                // Update the password hash to the new hashing mechanism
                storedHash = BCrypt.Net.BCrypt.HashPassword(password);

                // save the new hash to the database
                var user = _context.UmUserAccounts.FirstOrDefault(u => u.UserId == _userName);
                if (user != null)
                {
                    user.Password = storedHash;
                    _context.SaveChanges();
                }
                return true;
            }

            return false;
        }
    }

}
