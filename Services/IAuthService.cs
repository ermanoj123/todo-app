using System;
using System.Threading.Tasks;
using AuthAPI.Models;

namespace AuthAPI.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<User> RegisterAsync(RegisterRequest request, string? profileImageUrl = null);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> ChangePasswordAsync(string username, ChangePasswordRequest request);
        Task<User> UpdateProfileAsync(string username, UpdateProfileRequest request);
    }
}
