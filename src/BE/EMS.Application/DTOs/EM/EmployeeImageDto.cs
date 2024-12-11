using Microsoft.AspNetCore.Http;

namespace EMS.Application.DTOs.EM
{
    public class EmployeeImageDto
    {
        public string? EmployeeId { get; set; }
        public IFormFile? Image { get; set; }
    }
}
