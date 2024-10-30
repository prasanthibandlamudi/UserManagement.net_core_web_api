using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagementSystem.Data;
using UserManagementSystem.Models;
using UserManagementSystem.Data.Repositories.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace UserManagementSystem.Data.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Mst_Users
                .Where(u => u.ActiveStatus)
                .OrderByDescending(u => u.CreatedDate)
                .Include(u => u.Role)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            // Fetch the user and check for null
            var user = await _context.Mst_Users
            .Include(u=>u.Role)
            .FirstOrDefaultAsync(u=>u.UserId==userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            return user;
        }
        
        public async Task<User> GetUserByEmailAsync(string email)
        {
            // Retrieve the user by email and ensure they are active
            var user = await _context.Mst_Users
                .Include(u => u.Role) // Include the role, assuming a navigation property named Role
                .FirstOrDefaultAsync(u => u.Email == email && u.ActiveStatus);
            // Check if the user is null
            if (user == null)
            {
                // If the user is not found, throw an ArgumentException
                throw new ArgumentException($"User with email '{email}' not found or is inactive.");
            }
            return user; // Return the found user
        }

        public async Task AddUserAsync(User user)
        {
            // Check if the user object is null or if the email already exists
            if (user == null || await _context.Mst_Users.AnyAsync(u => u.Email == user.Email))
            {
                // Create an appropriate message based on which condition failed
                var message = user == null 
                ? "User cannot be null." 
                : $"Email '{user.Email}' already exists. Please choose a different email.";
                throw new ArgumentException(message, nameof(user));
            }
             // Set ActiveStatus to true if not already set
            user.ActiveStatus = true; // Ensure ActiveStatus is true
            await _context.Mst_Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
           // Check if user exists before updating
           // Validate user
            if (user == null || 
                !await _context.Mst_Users.AnyAsync(u => u.UserId == user.UserId) || 
                await _context.Mst_Users.AnyAsync(u => u.Email == user.Email && u.ActiveStatus && u.UserId != user.UserId))
            {
                var message = user == null
                    ? "User cannot be null."
                        : !await _context.Mst_Users.AnyAsync(u => u.UserId == user.UserId)
                            ? $"User with ID {user.UserId} not found."
                            : $"Email '{user.Email}' already exists for an active user. Please choose a different email.";

                throw new InvalidOperationException(message);
            }
            // Update the user
            _context.Mst_Users.Update(user);
            await _context.SaveChangesAsync();       
        }

        public async Task SoftDeleteUserAsync(int id)
        {
            // Check if the user exists
            var user = await _context.Mst_Users.FindAsync(id);
            if (user == null)
            {
                throw new InvalidOperationException($"User with ID {id} not found."); // This will be caught in the controller
            }
            // Soft delete the user
            user.ActiveStatus = false; // Assuming you're marking the user as inactive
            _context.Mst_Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
