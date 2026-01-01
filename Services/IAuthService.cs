using System;
using System.Threading.Tasks;
using AuthAPI.Models;

namespace AuthAPI.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<User> RegisterAsync(RegisterRequest request);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
