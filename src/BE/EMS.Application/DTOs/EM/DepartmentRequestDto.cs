namespace EMS.Application.DTOs.EM
{
    public class DepartmentRequestDto
    {
        public required string DepartmentName { get; set; } // Tên phòng ban
        public string? DepartmentManagerId { get; set; } // Mã quản lý phòng ban
    }
}
