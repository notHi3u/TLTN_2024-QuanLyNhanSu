namespace EMS.Application.DTOs.Account
{
    public class RoleResponseDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public IEnumerable<PermissionResponseDto> Permissions { get; set; } = new List<PermissionResponseDto>();

    }
}
