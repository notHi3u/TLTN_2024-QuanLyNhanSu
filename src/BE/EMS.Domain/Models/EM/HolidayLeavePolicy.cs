using System.ComponentModel.DataAnnotations;

namespace EMS.Domain.Models.EM
{
    public class HolidayLeavePolicy
    {
        [Key] // Marks Id as the primary key
        public int Id { get; set; }

        [Required] // Ensures EffectiveYear is always provided
        public int EffectiveYear { get; set; }

        // No data annotation is necessary for List<DateOnly>, but you may want to add validations on the list itself if required
        public List<DateOnly>? Holidays { get; set; }

        [Range(0, int.MaxValue)] // Ensures HolidayCount is a non-negative number
        public int HolidayCount { get; set; }
    }
}
