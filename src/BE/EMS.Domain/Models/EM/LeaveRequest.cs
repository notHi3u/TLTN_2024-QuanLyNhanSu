namespace EMS.Domain.Models.EM
{
    public class LeaveRequest
    {
        public long Id { get; set; }
        public required string EmployeeId { get; set; }
        public required string LeaveType { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
