namespace EMS.Application.DTOs.EM
{
    public class HolidayLeavePolicyResponseDto
    {
        public int Id { get; set; } // Mã quy định
        public int EffectiveYear { get; set; } // Năm áp dụng
        public List<DateOnly>? Holidays { get; set; } // Những ngày nghỉ
        public int HolidayCount { get; set; } // Số ngày nghỉ
        public int LeaveDayCount { get; set; }
    }
}
