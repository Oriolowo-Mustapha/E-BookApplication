using E_BookApplication.DTOs;

namespace E_BookApplication.Interface.Service
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto);
        Task<AuthResponseDTO> LoginAsync(LoginUserRequestModel loginDto);
        Task<UserDTO> GetUserByIdAsync(string userId);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
        Task<bool> AssignRoleAsync(string userId, string role);
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);
        Task UpdateFirstLoginDateAsync(string userId);
    }
}
