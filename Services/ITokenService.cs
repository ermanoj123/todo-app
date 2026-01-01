using AuthAPI.Models;

namespace AuthAPI.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
