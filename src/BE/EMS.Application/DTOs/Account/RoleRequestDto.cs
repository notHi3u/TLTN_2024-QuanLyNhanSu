namespace EMS.Application.DTOs.Account
{
    public class RoleRequestDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public IEnumerable<string> PermissionsIds { get; set; } = new List<string>();
    }
}
