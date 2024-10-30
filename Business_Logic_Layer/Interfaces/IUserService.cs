using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;

namespace UserManagementSystem.BusinessLayer.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<UserDto> GetUserByEmailAsync(string Email);
        Task AddUserAsync(UserDto userDto);
        Task UpdateUserAsync(int userId,UserUpdateDto userUpdateDto);
        Task SoftDeleteUserAsync(int userId);
        Task<bool> VerifyPasswordAsync(string email, string password);
    }
}
