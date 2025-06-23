using E_BookApplication.Models.Entities;

namespace E_BookApplication.Contract.Service
{
    public interface IJwtService
    {
        string GenerateToken(User user, IList<string> roles);
        string GetUserIdFromToken(string token);
        bool ValidateToken(string token);
    }
}
