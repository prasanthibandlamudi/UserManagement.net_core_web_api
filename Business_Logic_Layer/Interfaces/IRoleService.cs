using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementSystem.DTOs;

namespace UserManagementSystem.BusinessLayer.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(int roleId);
        Task AddRoleAsync(RoleDto roleDto);
        Task UpdateRoleAsync(int roleId, UpdateRoleDto updateRoleDto);
        Task SoftDeleteRoleAsync(int roleId);
    }
}
