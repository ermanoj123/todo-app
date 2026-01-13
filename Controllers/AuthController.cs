using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AuthAPI.Models;
using AuthAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly IWebHostEnvironment _environment;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, IWebHostEnvironment environment)
        {
            _authService = authService;
            _logger = logger;
            _environment = environment;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _authService.LoginAsync(request);

                if (response == null)
                    return Unauthorized(new { message = "Invalid username or password" });

                _logger.LogInformation($"User {request.Username} logged in successfully");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new { message = "An error occurred during login", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request, [FromForm] IFormFile? profileImage)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Handle profile image upload
                string? profileImageUrl = null;
                if (profileImage != null && profileImage.Length > 0)
                {
                    // Validate file type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(profileImage.FileName).ToLowerInvariant();

                    if (!allowedExtensions.Contains(extension))
                    {
                        return BadRequest(new { message = "Invalid file type. Only JPG, PNG, and GIF are allowed." });
                    }

                    // Validate file size (max 5MB)
                    if (profileImage.Length > 5 * 1024 * 1024)
                    {
                        return BadRequest(new { message = "File size must be less than 5MB." });
                    }

                    // Create uploads directory if it doesn't exist
                    var webRootPath = _environment.WebRootPath ?? Path.Combine(_environment.ContentRootPath, "wwwroot");
                    var uploadsFolder = Path.Combine(webRootPath, "uploads", "profiles");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Generate unique filename
                    var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await profileImage.CopyToAsync(fileStream);
                    }

                    // Store relative URL
                    profileImageUrl = $"/uploads/profiles/{uniqueFileName}";
                }

                var user = await _authService.RegisterAsync(request, profileImageUrl);

                if (user == null)
                    return BadRequest(new { message = "Username or email already exists" });

                _logger.LogInformation($"User {request.Username} registered successfully");

                return Ok(new { message = "User registered successfully", userId = user.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return StatusCode(500, new { message = "An error occurred during registration", error = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    return Unauthorized();

                var user = await _authService.GetUserByUsernameAsync(username);

                if (user == null)
                    return NotFound(new { message = "User not found" });

                return Ok(new
                {
                    user.Id,
                    user.Username,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.ProfileImageUrl,
                    user.CreatedAt,
                    user.LastLoginAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user profile");
                return StatusCode(500, new { message = "An error occurred" });
            }
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    return Unauthorized();

                var result = await _authService.ChangePasswordAsync(username, request);

                if (!result)
                    return BadRequest(new { message = "Current password is incorrect" });

                _logger.LogInformation($"User {username} changed password successfully");

                return Ok(new { message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return StatusCode(500, new { message = "An error occurred while changing password" });
            }
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var username = User.Identity?.Name;
                if (string.IsNullOrEmpty(username))
                    return Unauthorized();

                var updatedUser = await _authService.UpdateProfileAsync(username, request);

                if (updatedUser == null)
                    return BadRequest(new { message = "Email already exists or user not found" });

                _logger.LogInformation($"User {username} updated profile successfully");

                return Ok(new
                {
                    updatedUser.Id,
                    updatedUser.Username,
                    updatedUser.Email,
                    updatedUser.FirstName,
                    updatedUser.LastName,
                    updatedUser.ProfileImageUrl,
                    updatedUser.CreatedAt,
                    updatedUser.LastLoginAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating profile");
                return StatusCode(500, new { message = "An error occurred while updating profile" });
            }
        }
    }
}
