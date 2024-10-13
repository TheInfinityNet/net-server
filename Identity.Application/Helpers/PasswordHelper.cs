using Microsoft.AspNetCore.Identity;

namespace InfinityNetServer.Services.Identity.Application.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, password);
            return hashedPassword;
        }

        public static bool VerifyPassword(string hashedPassword, string password)
        {
            var passwordHasher = new PasswordHasher<object>();
            var result = passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
