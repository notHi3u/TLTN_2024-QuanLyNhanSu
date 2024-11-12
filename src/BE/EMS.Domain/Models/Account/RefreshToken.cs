using System;
using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Models.Account
{
    public class RefreshToken
    {
        [Key] // Optionally use this to mark the primary key if necessary
        public string Id { get; set; } // ID of the token (could be a hash of the token)

        [Required] // Ensure the UserId is always set
        public string UserId { get; set; } // ID of the user associated with the token

        [Required] // Ensure the CreatedAt field is always set
        public DateTime CreatedAt { get; set; } // The time when the token was created

        [Required] // Ensure the TokenCode is always set
        [MaxLength(512)] // Limit the token code length (adjust if needed)
        public string TokenCode { get; set; } // The actual token string

        [Required] // Ensure the Expiry date is always set
        public DateTime Expiry { get; set; } // The expiry time of the token

    }
}
