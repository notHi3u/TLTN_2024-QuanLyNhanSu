using System.Security.Claims;

namespace EMS.Domain.Models.Account
{
    public class Permission
    {
        public string Id { get; set; }
        public required string Name { get; set; }
        public string? DepartmentId { get; set; }
        public string? Description { get; set; }

        // Additional properties or methods can be added as needed
        public virtual List<RolePermission> RolePermissions { get; set; }
    }
}
