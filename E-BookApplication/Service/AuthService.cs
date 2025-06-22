namespace E_BookApplication.Service
{
    using Microsoft.AspNetCore.Identity;
    using AutoMapper;
    using E_BookApplication.Contract.Service;
    using E_BookApplication.DTOs;
    using E_BookApplication.Models.Entities;
    using Org.BouncyCastle.Crypto;

    namespace EBookStore.Services
    {
        public class AuthService : IAuthService
        {
            private readonly UserManager<User> _userManager;
            private readonly SignInManager<User> _signInManager;
            private readonly IJwtService _jwtService;
            private readonly IMapper _mapper;

            public AuthService(
                UserManager<User> userManager,
                SignInManager<User> signInManager,
                IJwtService jwtService,
                IMapper mapper)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtService = jwtService;
                _mapper = mapper;
            }

            public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto)
            {
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "User with this email already exists"
                    };
                }

                var user = new User
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    FullName = registerDto.FullName,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                // Assign default role
                await _userManager.AddToRoleAsync(user, "Customer");

                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtService.GenerateToken(user, roles);

                return new AuthResponseDTO
                {
                    Success = true,
                    Token = token,
                    User = _mapper.Map<UserDTO>(user),
                    Message = "Registration successful"
                };
            }

            public async Task<AuthResponseDTO> LoginAsync(LoginUserRequestModel loginDto)
            {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null || !user.IsActive)
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "Invalid credentials or account inactive"
                    };
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.PasswordHash, false);
                if (!result.Succeeded)
                {
                    return new AuthResponseDTO
                    {
                        Success = false,
                        Message = "Invalid credentials"
                    };
                }

                user.LastLoginAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtService.GenerateToken(user, roles);

                return new AuthResponseDTO
                {
                    Success = true,
                    Token = token,
                    User = _mapper.Map<UserDTO>(user),
                    Message = "Login successful"
                };
            }

            public async Task<UserDTO> GetUserByIdAsync(string userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                return user != null ? _mapper.Map<UserDTO>(user) : null;
            }

            public async Task<UserDTO> GetUserByEmailAsync(string email)
            {
                var user = await _userManager.FindByEmailAsync(email);
                return user != null ? _mapper.Map<UserDTO>(user) : null;
            }

            public async Task<bool> UserExistsAsync(string email)
            {
                return await _userManager.FindByEmailAsync(email) != null;
            }

            public async Task<bool> AssignRoleAsync(string userId, string role)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return false;

                var result = await _userManager.AddToRoleAsync(user, role);
                return result.Succeeded;
            }

            public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return new List<string>();

                return await _userManager.GetRolesAsync(user);
            }
        }
    }
}
