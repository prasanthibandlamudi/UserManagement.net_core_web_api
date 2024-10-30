using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementSystem.BusinessLayer.Interfaces;
using UserManagementSystem.DTOs;

namespace UserManagementSystem.Controllers
{
    [Authorize] // Require authentication for all actions in this controller
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // GET: api/role
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")] // Allow only Super Admin to fetch roles
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
        {
            try
            {
                // Call the service to fetch all roles
                var roles = await _roleService.GetAllRolesAsync();
                // If no roles are found, return an empty list
                if (roles == null || !roles.Any())
                {
                    return NotFound(new { message = "No roles found." });
                }
                // Return the roles with a 200 OK status
                return Ok(roles);
            }
            catch (Exception ex) // Catch any unexpected exceptions
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred while fetching the roles." });
            }
        }
        
        // GET: api/role/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "SuperAdmin")] // Allow only Super Admin to fetch a role by ID
        public async Task<ActionResult<RoleDto>> GetRoleById(int id)
        {
            try
            {
                // Call the service to fetch the role by ID
                var role = await _roleService.GetRoleByIdAsync(id);
                // If the role is found, return it
                return Ok(role); 
            }
            catch (KeyNotFoundException ex)
            {
                // Handle specific case where the role is not found
                return NotFound(new { message = ex.Message }); // Return NotFound with the exception message
            }
            catch (Exception ex) // Catch any other unexpected exceptions
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred while fetching the role." });
            }
        }

        // POST: api/role
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")] // Allow only Super Admin to create roles
        public async Task<ActionResult<RoleDto>> AddRole(RoleDto roleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try{
                await _roleService.AddRoleAsync(roleDto);
                return Ok(new { message = "Role created successfully."});
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific invalid operation errors (e.g., role name already exists)
                return BadRequest(new { message = ex.Message }); // Return BadRequest with the exception message
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                return StatusCode(500, new { message = "An unexpected error occurred while adding the role." });
            }
        }

        // PUT: api/role
        [HttpPut("{roleId}")]
        [Authorize(Roles = "SuperAdmin")] // Allow only Super Admin to update roles
        public async Task<IActionResult> UpdateRole(int roleId, [FromBody] UpdateRoleDto updateRoleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try{
            await _roleService.UpdateRoleAsync(roleId, updateRoleDto);
            return Ok(new { message = "Role updated successfully." }); // Return success message
            }
            catch (KeyNotFoundException ex)
            {
                // Handle case where user with the provided ID is not found
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                return StatusCode(500, new { message = "An unexpected error occurred while updating the user." });
            }  
        }

        // DELETE: api/role/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")] // Allow only Super Admin to soft delete roles
        public async Task<IActionResult> SoftDeleteRole(int id)
        {
            try
            {
                await _roleService.SoftDeleteRoleAsync(id);
                return Ok(new { message = "Role soft deleted successfully." }); // Return success message
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(new { message = ex.Message }); // Return BadRequest for invalid operations
            }
            catch (Exception ex) // Catch any other unexpected exceptions
            {
                return StatusCode(500, new { message = "An unexpected error occurred while deleting the role." });
            }
        }

    }
}
