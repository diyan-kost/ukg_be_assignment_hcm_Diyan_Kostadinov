using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HCM.Core.Helpers
{
    public static class CustomClaims
    {
        public const string EmployeeId = "EmployeeId";
    }

    public static class LoginHelper
    {
        /// <summary>
        /// Computes and returns the hash value of the text.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ComputeSHA256Hash(string text)
        {
            using (var sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "").ToLower();
            }
        }

        public static ClaimsPrincipal GenerateClaims(int userId, string username, string role, int employeeId)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(CustomClaims.EmployeeId, employeeId.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }

    }
}
