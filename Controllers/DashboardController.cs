using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AuthAPI.Models;
using AuthAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IAuthService authService, ILogger<DashboardController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Get dashboard data for the authenticated user
        /// </summary>
        /// <returns>Dashboard data including user info and statistics</returns>
        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                {
                    _logger.LogWarning("Unauthorized access attempt to dashboard");
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var user = await _authService.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    _logger.LogWarning($"User {username} not found");
                    return NotFound(new { message = "User not found" });
                }

                var dashboardData = new DashboardData
                {
                    User = new UserInfo
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    },
                    Stats = new UserStats
                    {
                        LastLogin = user.LastLoginAt ?? DateTime.UtcNow,
                        AccountCreated = user.CreatedAt,
                        AccountAgeDays = (DateTime.UtcNow - user.CreatedAt).Days,
                        IsActive = user.IsActive
                    }
                };

                _logger.LogInformation($"Dashboard data retrieved for user {username}");
                return Ok(dashboardData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard data");
                return StatusCode(500, new { message = "An error occurred while retrieving dashboard data" });
            }
        }

        /// <summary>
        /// Get user statistics
        /// </summary>
        /// <returns>User statistics</returns>
        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            try
            {
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    return Unauthorized();

                var user = await _authService.GetUserByUsernameAsync(username);
                if (user == null)
                    return NotFound(new { message = "User not found" });

                var stats = new UserStats
                {
                    LastLogin = user.LastLoginAt ?? DateTime.UtcNow,
                    AccountCreated = user.CreatedAt,
                    AccountAgeDays = (DateTime.UtcNow - user.CreatedAt).Days,
                    IsActive = user.IsActive
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user stats");
                return StatusCode(500, new { message = "An error occurred" });
            }
        }
    }
}
