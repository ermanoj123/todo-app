using System;
using System.Linq;
using AuthAPI.Models;
using System.Security.Cryptography;
using System.Text;

namespace AuthAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if we already have users
            if (context.Users.Any())
            {
                return; // DB has been seeded
            }

            // Seed sample users
            var users = new User[]
            {
                new User
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    PasswordHash = HashPassword("Admin@123"),
                    FirstName = "Admin",
                    LastName = "User",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                },
                new User
                {
                    Username = "demo",
                    Email = "demo@example.com",
                    PasswordHash = HashPassword("Demo@123"),
                    FirstName = "Demo",
                    LastName = "User",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
