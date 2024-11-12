using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Models.Account
{
    public class RolePermission
    {
        // Foreign Key for Role, must be required
        [Required]
        public string RoleId { get; set; }

        // Foreign Key for Permission, must be required
        [Required]
        public string PermissionId { get; set; }

        // Navigation property for Role (Required)
        [Required]
        public virtual Role Role { get; set; }

        // Navigation property for Permission (Required)
        [Required]
        public virtual Permission Permission { get; set; }
    }
}
