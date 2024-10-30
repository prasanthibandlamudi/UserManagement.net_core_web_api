using AutoMapper;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UserManagementSystem.BusinessLayer.Interfaces;
using UserManagementSystem.Data.Repositories.Interfaces;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;

namespace UserManagementSystem.BusinessLayer.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper,IEmailSenderService emailSenderService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _emailSenderService=emailSenderService;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto> GetUserByEmailAsync(string Email)
        {
            var user = await _userRepository.GetUserByEmailAsync(Email);
            return _mapper.Map<UserDto>(user);
        }

        public async Task AddUserAsync(UserDto userDto)
        {
            var plainTextPassword = userDto.Password; // Keep the plain text password for the email
            userDto.Password = HashPassword(userDto.Password); // Hash the password before saving
            var user = _mapper.Map<User>(userDto); // Map DTO to User model
            await _userRepository.AddUserAsync(user); // Add user to the repository
            // Send email notification with the plain text password
             await _emailSenderService.SendEmailNotification(user, plainTextPassword);   
        }
        
    
        public async Task UpdateUserAsync(int userId,UserUpdateDto userUpdateDto)
        {
            // Fetch the existing user
            var existingUser = await _userRepository.GetUserByIdAsync(userId);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            // Update only the fields that are provided
            if (!string.IsNullOrWhiteSpace(userUpdateDto.UserName))
            {
                existingUser.UserName = userUpdateDto.UserName;
            }
            if (!string.IsNullOrWhiteSpace(userUpdateDto.Email))
            {
                existingUser.Email = userUpdateDto.Email;
            }
            if (userUpdateDto.RoleId.HasValue)
            {
                existingUser.RoleId = userUpdateDto.RoleId.Value;
            }
            if (!string.IsNullOrWhiteSpace(userUpdateDto.PhoneNumber))
            {
                existingUser.PhoneNumber = userUpdateDto.PhoneNumber;
            }
            if (userUpdateDto.ActiveStatus.HasValue)
            {
                existingUser.ActiveStatus = userUpdateDto.ActiveStatus.Value;
            }
            if (!string.IsNullOrWhiteSpace(userUpdateDto.Password))
            {
                existingUser.Password = HashPassword(userUpdateDto.Password); // Ensure password is hashed
            }
            // Save changes to the repository
            await _userRepository.UpdateUserAsync(existingUser);
        }

        public async Task SoftDeleteUserAsync(int userId)
        {
            await _userRepository.SoftDeleteUserAsync(userId);
        }

        public async Task<bool> VerifyPasswordAsync(string email, string password)
        {
            // Fetch the user entity which includes the hashed password
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                return false; // User does not exist
            }
            // Compare the hashed password
            return VerifyHashedPassword(user.Password, password);
        }

        private bool VerifyHashedPassword(string hashedPassword, string password)
        {
            // Hash the input password and compare with the hashed password
            // Assuming you are using SHA256 for hashing; use the same algorithm you used for hashing the password.
            using (var sha256 = SHA256.Create())
            {
                var hashedInputPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return hashedPassword == hashedInputPassword;
            }
        }

        private string HashPassword(string password)
        {
            // Hash the password using SHA256
            using (var sha256 = SHA256.Create())
            {
                var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return hashedPassword;
            }
        }
    }
}
