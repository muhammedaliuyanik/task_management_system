using System;
using System.Security.Cryptography;

namespace TaskManagementSystem.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hashedBytes = hmac.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            var hashedInputPassword = HashPassword(password);
            return hashedInputPassword == hashedPassword;
        }
    }
}
