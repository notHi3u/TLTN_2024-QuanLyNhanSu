using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Models.Account
{
    public class Role : IdentityRole<string>
    {
        public Role() : base() { }

        public Role(string name)
            : base(name)
        {
        }

        // Navigation property for UserRoles
        public virtual ICollection<UserRole> UserRoles { get; set; }

        // Description property for additional information about the role
        [MaxLength(500)] // Limit the length of the description to 500 characters
        public string Description { get; set; } // Ví dụ thêm thuộc tính mô tả

        // Navigation property for Permissions
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        // Navigation property for RolePermissions
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
