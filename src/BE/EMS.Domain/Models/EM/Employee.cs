using EMS.Domain.Models.Account;

namespace EMS.Domain.Models.EM
{
    public class Employee
    {
        public string Id { get; set; } = new Guid().ToString(); // Mã nhân viên (Primary Key)
        public required string LastName { get; set; }// Họ
        public required string FirstName { get; set; }// Tên
        public DateOnly? DateOfBirth { get; set; }// Ngày sinh
        public required string Gender { get; set; }// Giới tính
        public required string Nationality { get; set; }// Quốc tịch
        public string? Address { get; set; }// Địa chỉ
        public string? PhoneNumber { get; set; }// Số điện thoại 
        public DateOnly HireDate { get; set; }// Ngày vào làm
        public DateOnly? FiredDate { get; set; }// Ngày rời cty (nullable)
        public string? Position { get; set; }// Vị trí công việc của nhân viên
        public EmployeeStatus? Status { get; set; }// Tình trạng hoạt động
        public string? MaritalStatus { get; set; }// Tình trạng hôn nhân
        public string? EducationLevel { get; set; }// Trình độ học vấn cao nhất
        public string? IdNumber { get; set; }// Mã số CCCD hoặc Passport
        public string? DepartmentId { get; set; }// Mã phòng ban (Foreign Key)
        public string? TaxId { get; set; }// Mã số thuế thu nhập cá nhân
        public required string Email { get; set; }// Email

        public string? UserId { get; set; }//Tk đc tạo sau nv, có thể đổi tk cho nv nếu mất tk

        public virtual User User { get; set; }
        public virtual ICollection<TimeCard> TimeCards { get; set; }
        public virtual ICollection<LeaveRequest> LeaveRequests { get; set; }
        public virtual ICollection<LeaveBalance> LeaveBalances { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }
        public virtual ICollection<EmployeeRelative> EmployeeRelatives { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual Department Department { get; set; }
        public virtual Salary Salary { get; set; }
        public virtual ICollection<SalaryHistory> SalaryHistory { get; set; }
        public virtual ICollection<WorkHistory> WorkHistories { get; set; }

    }
    public enum EmployeeStatus
    {
        Active,
        Inactive,
        OnLeave,
        Retired,
        Terminated
    }
}
