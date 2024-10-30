using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
        public DateTime CreatedDate { get; set; }=DateTime.UtcNow;
        [Required]
        public string CreatedBy { get; set; }
        [Required]
        public bool ActiveStatus { get; set; } = true; // Default to active

        public ICollection<User> Users { get; set; } // Navigation property
    }
}
