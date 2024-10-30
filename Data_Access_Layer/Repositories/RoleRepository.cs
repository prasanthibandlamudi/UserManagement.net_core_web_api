using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementSystem.Data;
using UserManagementSystem.Models;
using UserManagementSystem.Data.Repositories.Interfaces;

namespace UserManagementSystem.Data.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _context.Mst_Roles
                .Where(r => r.ActiveStatus)
                .ToListAsync();
        }
        
        public async Task<Role> GetRoleByIdAsync(int roleId)
        {
            // Fetch the role and check for null
            var role = await _context.Mst_Roles.FindAsync(roleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {roleId} not found.");
            }
            return role;
        }

        public async Task AddRoleAsync(Role role)
        {
            // Combined check for null, allowed role names, and existence of active roles with the same name
            if (role == null || 
                await _context.Mst_Roles.AnyAsync(r => r.RoleName == role.RoleName && r.ActiveStatus))
            {
                var message = role == null
                    ? "Role cannot be null."
                        : $"Role name '{role.RoleName}' already exists. Please choose a different name.";
                throw new InvalidOperationException(message);
            }
            // If all checks pass, add the role
            await _context.Mst_Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(Role role)
        {
            // Validate role
            if (role == null || 
                !await _context.Mst_Roles.AnyAsync(r => r.RoleId == role.RoleId) || 
                await _context.Mst_Roles.AnyAsync(r => r.RoleName == role.RoleName && r.ActiveStatus && r.RoleId != role.RoleId))
            {
                var message = role == null
                    ? "Role cannot be null."
                        : !await _context.Mst_Roles.AnyAsync(r => r.RoleId == role.RoleId)
                            ? $"Role with ID {role.RoleId} not found."
                            : $"Role name '{role.RoleName}' already exists for an active role. Please choose a different name.";

                throw new InvalidOperationException(message);
            }
            // Update the role
            _context.Mst_Roles.Update(role);
            await _context.SaveChangesAsync();
        }
        
        public async Task SoftDeleteRoleAsync(int roleId)
        {
            var role = await GetRoleByIdAsync(roleId);
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with ID {roleId} not found."); // This will be caught in the controller
            }
            {
                role.ActiveStatus = false; // Soft delete
                await UpdateRoleAsync(role);
            }
        }
    }
}
