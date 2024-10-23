namespace EMS.Domain.Models.Account
{
    public class RolePermission
    {
        public string RoleId { get; set; } // Assuming the RoleId is of long type
        public string PermissionId { get; set; } // Assuming the PermissionId is also of long type

        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
}

