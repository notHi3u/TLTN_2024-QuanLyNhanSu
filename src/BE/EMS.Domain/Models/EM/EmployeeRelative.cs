namespace EMS.Domain.Models.EM
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

        public virtual Employee Employee { get; set; }
    }
}
