using Microsoft.AspNetCore.Identity;

namespace EMS.Domain.Models.Account
{
    public class Role : IdentityRole<string>
    {
        public Role() : base() { }

        public Role(string name)
            : base(name)
        {
        }

        public virtual ICollection<UserRole> UserRoles { get; set; }
        public string Description { get; set; } // Ví dụ thêm thuộc tính mô tả
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
