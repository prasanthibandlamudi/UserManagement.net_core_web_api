using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementSystem.BusinessLayer.Interfaces;
using UserManagementSystem.Data.Repositories.Interfaces;
using UserManagementSystem.DTOs;
using UserManagementSystem.Models;
using AutoMapper;

namespace UserManagementSystem.BusinessLayer.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<RoleDto> GetRoleByIdAsync(int roleId)
        {
            var role = await _roleRepository.GetRoleByIdAsync(roleId);
            return _mapper.Map<RoleDto>(role);
        }

        public async Task AddRoleAsync(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            await _roleRepository.AddRoleAsync(role);
        }

        public async Task UpdateRoleAsync(int roleId, UpdateRoleDto updateRoleDto)
        {
            // Fetch the existing role
            var existingRole = await _roleRepository.GetRoleByIdAsync(roleId);
            // Update only the fields that are provided
            if (!string.IsNullOrWhiteSpace(updateRoleDto.RoleName))
            {
                existingRole.RoleName = updateRoleDto.RoleName;
            }
            if (!string.IsNullOrWhiteSpace(updateRoleDto.CreatedBy))
            {
                existingRole.CreatedBy = updateRoleDto.CreatedBy;
            }
            if (updateRoleDto.ActiveStatus.HasValue)
            {
                existingRole.ActiveStatus = updateRoleDto.ActiveStatus.Value;
            }
            // Save changes
            await _roleRepository.UpdateRoleAsync(existingRole);
        }

        public async Task SoftDeleteRoleAsync(int roleId)
        {
            await _roleRepository.SoftDeleteRoleAsync(roleId);
        }
    }
}
