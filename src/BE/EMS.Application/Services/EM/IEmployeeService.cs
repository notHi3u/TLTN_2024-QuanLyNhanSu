﻿using Common.Dtos;
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
    }
}
