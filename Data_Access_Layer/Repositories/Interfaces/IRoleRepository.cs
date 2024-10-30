using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementSystem.Models;

namespace UserManagementSystem.Data.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int roleId);
        Task AddRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task SoftDeleteRoleAsync(int roleId);
    }
}
