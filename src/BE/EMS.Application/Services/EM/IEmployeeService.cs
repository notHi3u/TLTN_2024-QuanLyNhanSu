using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface IEmployeeService
    {
        Task<EmployeeResponseDto> GetEmployeeByIdAsync(string id);
        Task<EmployeeResponseDto> CreateEmployeeAsync(EmployeeRequestDto employeeRequestDto);
        Task<EmployeeResponseDto> UpdateEmployeeAsync(string id, EmployeeRequestDto employeeRequestDto);
        Task<bool> DeleteEmployeeAsync(string id);
        Task<PagedDto<EmployeeResponseDto>> GetPagedEmployeesAsync(EmployeeFilter filter);
        Task<bool> AssignDepartmentAsync(string id, string departmentId);
        Task<bool> RemoveDepartmentAsync(string id);
        Task<EmployeeResponseDto> BindUserToEmployeeAsync(string employeeId, string userId);
        Task<EmployeeResponseDto> UnBindUserToEmployeeAsync(string employeeId);
        Task<decimal> GetTotalSalaryAsync();
        Task<string> SaveEmployeeImage(EmployeeImageDto employeeImageDto);
    }
}
