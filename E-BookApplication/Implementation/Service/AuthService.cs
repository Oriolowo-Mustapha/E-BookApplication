using AutoMapper;
using E_BookApplication.DTOs;
using E_BookApplication.Interface.Service;
using E_BookApplication.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

namespace E_BookApplication.Implementation.Service
{
	public class AuthService : IAuthService
	{
		private readonly UserManager<User> _userManager;

		private readonly SignInManager<User> _signInManager;
		private readonly IJwtService _jwtService;
		private readonly IMapper _mapper;
		private readonly ILogger<AuthService> _logger;

		public AuthService(
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			IJwtService jwtService,
			IMapper mapper,
			ILogger<AuthService> logger)

		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtService = jwtService;
			_mapper = mapper;
			_logger = logger;

		}

		public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDto)
		{
			try
			{
				// Validate input
				if (string.IsNullOrWhiteSpace(registerDto.Email) ||
					string.IsNullOrWhiteSpace(registerDto.Password) ||
					string.IsNullOrWhiteSpace(registerDto.FullName))
				{
					return new AuthResponseDTO
					{
						Success = false,
						Message = "All fields are required"
					};
				}

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
					CreatedAt = DateTime.UtcNow,
					IsActive = true
				};


				var result = await _userManager.CreateAsync(user, registerDto.Password);
				if (!result.Succeeded)
				{
					var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
					_logger.LogWarning("User registration failed for {Email}: {Errors}", registerDto.Email, errorMessage);

					return new AuthResponseDTO
					{
						Success = false,
						Message = errorMessage
					};
				}


				var roleResult = await _userManager.AddToRoleAsync(user, Roles.Customer);
				if (!roleResult.Succeeded)
				{
					_logger.LogWarning("Failed to assign role to user {UserId}", user.Id);
				}

				var roles = await _userManager.GetRolesAsync(user);
				var token = _jwtService.GenerateToken(user, roles);

				_logger.LogInformation("User {Email} registered successfully", registerDto.Email);

				return new AuthResponseDTO
				{
					Success = true,
					Token = token,
					User = _mapper.Map<UserDTO>(user),
					Message = "Registration successful"
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during user registration for {Email}", registerDto.Email);
				return new AuthResponseDTO
				{
					Success = false,
					Message = "An error occurred during registration"
				};
			}
		}

		public async Task UpdateFirstLoginDateAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (user != null && user.FirstLoginDate == null)
			{
				user.FirstLoginDate = DateTime.UtcNow;
				await _userManager.UpdateAsync(user);
			}
		}

		public async Task<AuthResponseDTO> LoginAsync(LoginUserRequestModel loginDto)
		{
			try
			{

				if (string.IsNullOrWhiteSpace(loginDto.Email) ||
					string.IsNullOrWhiteSpace(loginDto.PasswordHash))
				{
					return new AuthResponseDTO
					{
						Success = false,
						Message = "Email and password are required"
					};
				}

				var user = await _userManager.FindByEmailAsync(loginDto.Email);
				if (user == null || !user.IsActive)
				{

					return new AuthResponseDTO
					{
						Success = false,
						Message = "Invalid credentials"
					};
				}


				var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.PasswordHash, lockoutOnFailure: true);

				if (result.IsLockedOut)
				{
					_logger.LogWarning("Account locked out for user {Email}", loginDto.Email);
					return new AuthResponseDTO
					{
						Success = false,
						Message = "Account is locked out. Please try again later."
					};
				}

				if (!result.Succeeded)
				{
					_logger.LogWarning("Login failed for user {Email}", loginDto.Email);
					return new AuthResponseDTO
					{
						Success = false,
						Message = "Invalid credentials"
					};
				}

				// Update last login
				user.LastLoginAt = DateTime.UtcNow;
				await _userManager.UpdateAsync(user);

				var roles = await _userManager.GetRolesAsync(user);
				var token = _jwtService.GenerateToken(user, roles);

				_logger.LogInformation("User {Email} logged in successfully", loginDto.Email);

				return new AuthResponseDTO
				{
					Success = true,
					Token = token,
					User = _mapper.Map<UserDTO>(user),
					Message = "Login successful"
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error during login for {Email}", loginDto.Email);
				return new AuthResponseDTO
				{
					Success = false,
					Message = "An error occurred during login"
				};
			}
		}


		private static string HashPasswordCustom(string password)
		{
			using var sha512 = SHA512.Create();
			var hashBytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToHexString(hashBytes).ToLowerInvariant();
		}

		public async Task<UserDTO?> GetUserByIdAsync(string userId)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(userId);
				return user != null ? _mapper.Map<UserDTO>(user) : null;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving user by ID {UserId}", userId);
				return null;
			}
		}

		public async Task<UserDTO?> GetUserByEmailAsync(string email)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(email);
				return user != null ? _mapper.Map<UserDTO>(user) : null;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving user by email {Email}", email);
				return null;
			}
		}

		public async Task<bool> UserExistsAsync(string email)
		{
			try
			{
				return await _userManager.FindByEmailAsync(email) != null;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error checking if user exists for email {Email}", email);
				return false;
			}
		}

		public async Task<bool> AssignRoleAsync(string userId, string role)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(userId);
				if (user == null)
				{
					_logger.LogWarning("User not found when trying to assign role {Role} to user {UserId}", role, userId);
					return false;
				}


				if (!IsValidRole(role))
				{
					_logger.LogWarning("Invalid role {Role} attempted for user {UserId}", role, userId);
					return false;
				}

				var result = await _userManager.AddToRoleAsync(user, role);
				if (result.Succeeded)
				{
					_logger.LogInformation("Role {Role} assigned to user {UserId}", role, userId);
				}
				else
				{
					_logger.LogWarning("Failed to assign role {Role} to user {UserId}: {Errors}",
						role, userId, string.Join(", ", result.Errors.Select(e => e.Description)));
				}

				return result.Succeeded;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error assigning role {Role} to user {UserId}", role, userId);
				return false;
			}
		}

		public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(userId);
				if (user == null)
				{
					_logger.LogWarning("User not found when getting roles for user {UserId}", userId);
					return new List<string>();
				}

				return await _userManager.GetRolesAsync(user);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error retrieving roles for user {UserId}", userId);
				return new List<string>();
			}
		}

		public async Task<AuthResponseDTO> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(userId);
				if (user == null)
				{
					return new AuthResponseDTO
					{
						Success = false,
						Message = "User not found"
					};
				}

				var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
				if (!result.Succeeded)
				{
					return new AuthResponseDTO
					{
						Success = false,
						Message = string.Join(", ", result.Errors.Select(e => e.Description))
					};
				}

				_logger.LogInformation("Password changed successfully for user {UserId}", userId);

				return new AuthResponseDTO
				{
					Success = true,
					Message = "Password changed successfully"
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error changing password for user {UserId}", userId);
				return new AuthResponseDTO
				{
					Success = false,
					Message = "An error occurred while changing password"
				};
			}
		}

		public async Task<bool> DeactivateUserAsync(string userId)
		{
			try
			{
				var user = await _userManager.FindByIdAsync(userId);
				if (user == null) return false;

				user.IsActive = false;
				var result = await _userManager.UpdateAsync(user);

				if (result.Succeeded)
				{
					_logger.LogInformation("User {UserId} deactivated", userId);
				}

				return result.Succeeded;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error deactivating user {UserId}", userId);
				return false;
			}
		}

		private static bool IsValidRole(string role)
		{
			return role == Roles.Admin || role == Roles.Vendor || role == Roles.Customer;
		}
	}
}
