using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Infrastructure.Repositories
{
    public class EmployeeRelative
    {
        public int Id { get; set; }
        public required string EmployeeId { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public required string Relationship { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Adress { get; set; }
        public bool EmergencyContact { get; set; }
    }
}
