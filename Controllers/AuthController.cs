using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagementSystem.BusinessLayer.Interfaces;
using UserManagementSystem.DTOs;
using UserManagementSystem.Services;

namespace UserManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenService _tokenService;

        public AuthController(IUserService userService, TokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // Fetch the user by email
            var user = await _userService.GetUserByEmailAsync(loginDto.Email);
              // Check if user exists and passwords match
            if (user == null || !await _userService.VerifyPasswordAsync(loginDto.Email, loginDto.Password))
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }
            // Generate JWT token for the user
           var token = _tokenService.GenerateToken(user.Email, user.RoleName);
           return Ok(new { token });
        }
    }
}
