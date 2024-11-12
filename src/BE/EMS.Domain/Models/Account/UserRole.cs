using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Models.Account
{
    public class UserRole : IdentityUserRole<string>
    {
        // Navigation properties for User and Role
        [Required] // Ensures that User navigation property is required
        public virtual User User { get; set; }

        [Required] // Ensures that Role navigation property is required
        public virtual Role Role { get; set; }


    }
}
