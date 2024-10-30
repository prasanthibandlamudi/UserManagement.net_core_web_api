namespace UserManagementSystem.DTOs
{
    public class UserUpdateDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? ActiveStatus { get; set; }
        public string? Password { get; set; }
    }
}
