using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface IDepartmentService
    {
        Task<DepartmentResponseDto> GetDepartmentByIdAsync(string id);
        Task<DepartmentResponseDto> CreateDepartmentAsync(DepartmentRequestDto departmentRequestDto);
        Task<DepartmentResponseDto> UpdateDepartmentAsync(string id, DepartmentRequestDto departmentRequestDto);
        Task<bool> DeleteDepartmentAsync(string id);
        Task<PagedDto<DepartmentResponseDto>> GetPagedDepartmentsAsync(DepartmentFilter filter);
        Task<DepartmentResponseDto> AssignManagerAsync(string departmentId, string managerId);
        Task<List<EmployeeResponseDto>> GetEmployeesByDepartmentAsync(string departmentId);
        Task<EmployeeResponseDto> GetDepartmentManagerAsync(string departmentId);
    }
}
