using EMS.Domain.Models.EM;
using Microsoft.AspNetCore.Identity;

namespace EMS.Domain.Models.Account
{
    public class User : IdentityUser
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual Employee Employee { get; set; }
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
