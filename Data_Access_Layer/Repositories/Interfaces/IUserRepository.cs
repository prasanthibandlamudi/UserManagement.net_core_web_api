using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementSystem.Models;

namespace UserManagementSystem.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int userId);
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task SoftDeleteUserAsync(int userId);
    }
}
