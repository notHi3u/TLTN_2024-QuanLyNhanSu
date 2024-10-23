namespace EMS.Domain.Models.EM
{
    public class Department
    {
        public required string Id { get; set; } // Mã phòng ban
        public required string DepartmentName { get; set; } // Tên phòng ban
        public string? DepartmentManagerId { get; set; } // Mã quản lý phòng ban

        public virtual ICollection<Employee> Employees { get; set; }
        public virtual Employee Manager { get; set; }
    }
}
