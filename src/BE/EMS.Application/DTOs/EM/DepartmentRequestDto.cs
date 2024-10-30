namespace EMS.Application.DTOs.EM
{
    public class DepartmentRequestDto
    {
        public required string Id { get; set; } // Mã phòng ban
        public required string DepartmentName { get; set; } // Tên phòng ban
        public string? DepartmentManagerId { get; set; } // Mã quản lý phòng ban
    }
}
