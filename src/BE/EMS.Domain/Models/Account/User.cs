using Microsoft.AspNetCore.Identity;

namespace EMS.Domain.Models.Account
{
    public class User : IdentityUser
    {
        public required string EmployeeId { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
