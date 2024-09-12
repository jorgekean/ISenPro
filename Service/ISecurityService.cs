using System.Threading.Tasks;

namespace Service
{
    public interface ISecurityService
    {
        // Login method to authenticate a user
        Task<AuthResult> LoginAsync(string username, string password);

        // Logout method to sign out a user
        Task LogoutAsync(string userId);

        // Method to refresh the JWT token
        Task<AuthResult> RefreshTokenAsync(string refreshToken);

        // Method to register a new user
        //Task<RegisterResult> RegisterAsync(string username, string password, string email);

        // Method to change user password
        Task ChangePasswordAsync(string userId, string currentPassword, string newPassword);

        // Method to confirm user's email
        //Task ConfirmEmailAsync(string userId, string token);

        // Method to initiate password reset
        Task InitiatePasswordResetAsync(string email);

        // Method to reset password
        Task ResetPasswordAsync(string userId, string token, string newPassword);

        // Method to check if the user is authorized for a particular action
        Task<bool> IsUserAuthorizedAsync(string userId, string action);
    }

    // Sample result classes
    public class AuthResult
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
    //public class RegisterResult
    //{
    //    public bool Success { get; set; }
    //    public IEnumerable<string> Errors { get; set; }
    //}
}
