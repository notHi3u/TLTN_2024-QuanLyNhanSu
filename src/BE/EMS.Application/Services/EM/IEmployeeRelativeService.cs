using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface IEmployeeRelativeService
    {
        Task<EmployeeRelativeResponseDto> GetEmployeeRelativeByIdAsync(string id);
        Task<EmployeeRelativeResponseDto> CreateEmployeeRelativeAsync(EmployeeRelativeRequestDto employeeRelativeRequestDto);
        Task<EmployeeRelativeResponseDto> UpdateEmployeeRelativeAsync(string id, EmployeeRelativeRequestDto employeeRelativeRequestDto);
        Task<bool> DeleteEmployeeRelativeAsync(string id);
        Task<PagedDto<EmployeeRelativeResponseDto>> GetPagedEmployeeRelativesAsync(EmployeeRelativeFilter filter);
    }
}
