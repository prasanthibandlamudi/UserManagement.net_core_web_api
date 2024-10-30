using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Models;

namespace UserManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Mst_Users { get; set; }
        public DbSet<Role> Mst_Roles { get; set; }
    
     protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "SuperAdmin", CreatedBy = "System", CreatedDate = DateTime.UtcNow,  ActiveStatus= true }
            );

            // Seed Super Admin User (Use hashed password in real applications)
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    UserId = 1, 
                    UserName = "superadmin", 
                    Email = "superadmin@gmail.com", 
                    RoleId = 1,  
                    CreatedDate = DateTime.UtcNow, 
                    CreatedBy = "System", 
                    ActiveStatus = true, 
                     Password = HashPassword("superadmin@123") 
                }
            );
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
                return hashedPassword;
            }
        }
    }
}