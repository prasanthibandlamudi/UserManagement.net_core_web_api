using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementSystem.BusinessLayer.Interfaces;
using UserManagementSystem.DTOs;

namespace UserManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/user
        [HttpGet]
        [Authorize(Roles = "SuperAdmin, Admin")] // Allow Super Admin and Admin to fetch users
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            try
            {
                // Call the service to fetch all active users
                var users = await _userService.GetAllUsersAsync();
                // If no users are found, return a NotFound response
                if (users == null || !users.Any())
                {
                    return NotFound(new { message = "No users found." });
                }
                // Return the users with a 200 OK status
                return Ok(users);
            }
            catch (Exception ex) // Catch any unexpected exceptions
            {
                // Handle unexpected errors
                return StatusCode(500, new { message = "An unexpected error occurred while fetching the users." });
            }
        }

        // GET: api/user/{id}
        [HttpGet("{id:int}")]
        [Authorize(Roles = "SuperAdmin, Admin")] // Allow Super Admin and Admin to fetch a user by ID
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                 return Ok(user); // Return the user data
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // Return NotFound with the exception message
            }
            catch (Exception ex) // Catch any unexpected exceptions
            {
                return StatusCode(500, new { message = "An unexpected error occurred while fetching the user." });
            }
        }
       
        // GET: api/user/{email}
        [HttpGet("{email}")]
        [Authorize(Roles = "SuperAdmin, Admin")] // Allow Super Admin and Admin to fetch a user by email
        public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                return Ok(user); // Return the found user
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message }); // Return NotFound with the exception message
            }
            catch (Exception ex)
            {
                 // Handle any other unexpected exceptions
                return StatusCode(500, new { message = "An unexpected error occurred while retrieving the user." }); // Return a 500 error for unexpected issues
            }
        }

        // POST: api/user
        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")] // Allow Super Admin and Admin to create users
        public async Task<ActionResult<UserDto>> AddUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                // Call the service to add the user
                await _userService.AddUserAsync(userDto);
                return Ok(new { message = "User created successfully." });
            }
            catch (ArgumentException ex) when (ex.ParamName == "user")
            {
                // Catch only the ArgumentException specific to duplicate emails
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                return StatusCode(500, new { message = "An unexpected error occurred while adding the user." });
            }  
        }

        // PUT: api/user/{id}
        [HttpPut("{id:int}")]
        [Authorize(Roles = "SuperAdmin")] // Restrict update permissions as needed
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _userService.UpdateUserAsync(id, userUpdateDto);
                return Ok(new { message = "User updated successfully." });
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

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "SuperAdmin")] // Allow only Super Admin to soft delete users
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            try
            {
                // Attempt to soft delete the user
                await _userService.SoftDeleteUserAsync(id);
                return Ok(new { message = "User soft deleted successfully." }); // Return success message
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific invalid operation errors (e.g., user not found)
                return NotFound(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions
                return StatusCode(500, new { message = "An unexpected error occurred while attempting to soft delete the user." });
            }
        }
    }
}
