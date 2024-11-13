namespace EMS.Application.DTOs.EM
{
    public class HolidayLeavePolicyRequestDto
    {
        public int EffectiveYear { get; set; } // Năm áp dụng
        public List<DateOnly>? Holidays { get; set; } // Những ngày nghỉ
    }
}
