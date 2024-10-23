namespace EMS.Domain.Models.EM
{
    public class LeaveBalance
    {
        public long Id { get; set; }
        public required string EmployeeId { get; set; }
        public int Year {  get; set; }
        public int LeaveDayCount { get; set; }
        public int UsedLeaveDays { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
