namespace UserManagementSystem.DTOs
{
    public class UpdateRoleDto
    {
        public string? RoleName { get; set; }
        public string? CreatedBy { get; set; }
        public bool? ActiveStatus{ get; set;}
    }
}
