namespace UserManagementSystem.DTOs
{
    public class UserCreateDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
