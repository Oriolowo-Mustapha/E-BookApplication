using E_BookApplication.Models.Entities;

namespace E_BookApplication.Interface.Service
{
    public interface IJwtService
    {
        string GenerateToken(User user, IList<string> roles);
        string GetUserIdFromToken(string token);
        bool ValidateToken(string token);
    }
}
