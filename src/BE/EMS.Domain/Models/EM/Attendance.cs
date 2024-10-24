namespace EMS.Domain.Models.EM
{
    public class Attendance
    {
        public long Id { get; set; }
        public required string EmployeeId { get; set; }
        public required DateOnly Date { get; set; }
        public required bool WorkStatus { get; set; }
        public string? AbsentReasons { get; set; }
        public bool Status { get; set; } = false;
        public long TimeCardId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual TimeCard TimeCard { get; set; }
    }
}
