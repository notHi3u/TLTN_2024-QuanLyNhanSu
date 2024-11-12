using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Models.Account
{
    public class Permission
    {
        [Key] // Optionally use this if you need to explicitly mark the ID property as the primary key
        public string Id { get; set; }

        [Required] // Ensures the Name is always provided
        [MaxLength(255)] // You can adjust this length based on your requirements
        public string Name { get; set; }

        [MaxLength(500)] // You can limit the description length if needed
        public string? Description { get; set; } // Description is optional

        // Collection for RolePermissions (no need for annotation here, just a relationship)
        public virtual List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
