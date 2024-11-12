using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface IEmployeeRelativeService
    {
        Task<EmployeeRelativeResponseDto> GetEmployeeRelativeByIdAsync(int id);
        Task<EmployeeRelativeResponseDto> CreateEmployeeRelativeAsync(EmployeeRelativeRequestDto employeeRelativeRequestDto);
        Task<EmployeeRelativeResponseDto> UpdateEmployeeRelativeAsync(int id, EmployeeRelativeRequestDto employeeRelativeRequestDto);
        Task<bool> DeleteEmployeeRelativeAsync(int id);
        Task<PagedDto<EmployeeRelativeResponseDto>> GetPagedEmployeeRelativesAsync(EmployeeRelativeFilter filter);
        Task<IEnumerable<EmployeeRelativeResponseDto>> GetEmployeeRelativesByEmployeeIdAsync(string employeeId);
    }
}
