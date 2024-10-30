using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.DTOs
{
    public class UserDto
    {
        public int UserId{get;set;}
        public string UserName { get; set; }
         [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public bool ActiveStatus { get; set; }
        public string Password{ get; set;}
         [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }
    }
}
