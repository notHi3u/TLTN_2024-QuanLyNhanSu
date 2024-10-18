using Microsoft.AspNetCore.Identity;

namespace EMS.Domain.Models.Account
{
    public class User : IdentityUser
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }

    }
}
