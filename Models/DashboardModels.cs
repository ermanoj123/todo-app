using System;

namespace AuthAPI.Models
{
    public class DashboardData
    {
        public UserInfo User { get; set; } = null!;
        public UserStats Stats { get; set; } = null!;
    }

    public class UserInfo
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class UserStats
    {
        public DateTime LastLogin { get; set; }
        public DateTime AccountCreated { get; set; }
        public int AccountAgeDays { get; set; }
        public bool IsActive { get; set; }
    }
}
