using EMS.Domain.Models.EM;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Models.Account
{
    public class User : IdentityUser
    {
        // Navigation property for UserRoles relationship (Many-to-Many)
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // Navigation property for the relationship with Employee (One-to-One or One-to-Many based on your requirements)
        public virtual Employee Employee { get; set; }

        // Permissions associated with the user
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    }
}
