using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Application.Services.EM
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepository _employeeRepository;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper, IEmployeeRepository employeeRepository)
        {
            _departmentRepository = departmentRepository ?? throw new ArgumentNullException(nameof(departmentRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException( nameof(employeeRepository));
        }

        #region Create
        public async Task<DepartmentResponseDto> CreateDepartmentAsync(DepartmentRequestDto departmentRequestDto)
        {
            if (departmentRequestDto == null)
                throw new ArgumentNullException(nameof(departmentRequestDto));

            Department department = _mapper.Map<Department>(departmentRequestDto);
            department.Id = Guid.NewGuid().ToString();

            if (await _departmentRepository.ExistsAsync(d => d.DepartmentName == department.DepartmentName))
            {
                throw new ArgumentException("A department with the same name already exists");
            }

            await _departmentRepository.AddAsync(department);
            return _mapper.Map<DepartmentResponseDto>(department);
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteDepartmentAsync(string id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            await _departmentRepository.DeleteAsync(department);
            return true;
        }
        #endregion

        #region Get by Id
        public async Task<DepartmentResponseDto> GetDepartmentByIdAsync(string id, bool? isDeep)
        {
            var department = await _departmentRepository.GetByIdAsync(id, isDeep);
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            return _mapper.Map<DepartmentResponseDto>(department);
        }
        #endregion

        #region Get Paged
        public async Task<PagedDto<DepartmentResponseDto>> GetPagedDepartmentsAsync(DepartmentFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedDepartments = await _departmentRepository.GetPagedAsync(filter);

            var departmentResponseDtos = _mapper.Map<IEnumerable<DepartmentResponseDto>>(pagedDepartments.Items);

            return new PagedDto<DepartmentResponseDto>(
                departmentResponseDtos,
                pagedDepartments.TotalCount,
                pagedDepartments.PageIndex,
                pagedDepartments.PageSize
            );
        }
        #endregion

        #region Update
        public async Task<DepartmentResponseDto> UpdateDepartmentAsync(string id, DepartmentRequestDto departmentRequestDto)
        {
            if (departmentRequestDto == null)
                throw new ArgumentNullException(nameof(departmentRequestDto));

            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            _mapper.Map(departmentRequestDto, department);

            if (await _departmentRepository.ExistsAsync(d => d.DepartmentName == department.DepartmentName && d.Id != id))
            {
                throw new ArgumentException("A department with the same name already exists");
            }

            await _departmentRepository.UpdateAsync(department);
            return _mapper.Map<DepartmentResponseDto>(department);
        }
        #endregion

        #region Assign Manager Async
        public async Task<DepartmentResponseDto> AssignManagerAsync(string departmentId, string managerId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            var oldManager = await GetDepartmentManagerAsync(departmentId);
            var manager = await _employeeRepository.GetByIdAsync(managerId);
            if(department == null)
                throw new ArgumentNullException(nameof(department));

            if (manager == null)
            {
                throw new ArgumentNullException(nameof(managerId));
            }
            
            if(oldManager != null)
            {
                oldManager.DepartmentId = null;
                oldManager.Position = null;
            }
            department.DepartmentManagerId = managerId;
            manager.DepartmentId = departmentId;
            manager.Position = "Manager";

            await _departmentRepository.UpdateAsync(department);
            await _employeeRepository.UpdateAsync(manager);

            return _mapper.Map<DepartmentResponseDto>(department);

        }
        #endregion

        public async Task<DepartmentResponseDto> RemoveManagerAsync(string departmentId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);
            var manager = await _employeeRepository.GetByIdAsync(department.DepartmentManagerId);
            if (department == null)
                throw new ArgumentNullException(nameof(department));


            if (department.DepartmentManagerId == null)
            {
                throw new ArgumentNullException("No manager found");
            }
            
            department.DepartmentManagerId = null;
            manager.Position= null;
            manager.DepartmentId = null;


            await _departmentRepository.UpdateAsync(department);
            await _employeeRepository.UpdateAsync(manager);
            return _mapper.Map<DepartmentResponseDto>(department);
        }

        #region Get Employees By Department
        public async Task<IEnumerable<EmployeeResponseDto>> GetEmployeesByDepartmentAsync(string departmentId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);

            if (department == null)
                throw new ArgumentNullException(nameof(department));

            // Fetch employees by the departmentId
            var employees = await _employeeRepository.GetByDepartmentIdAsync(departmentId);

            // Map to response DTOs
            return _mapper.Map<IEnumerable<EmployeeResponseDto>>(employees);
        }
        #endregion

        #region Get Department Manager
        public async Task<EmployeeResponseDto> GetDepartmentManagerAsync(string departmentId)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentId);

            if (department == null)
                throw new ArgumentNullException(nameof(department));

            if (string.IsNullOrEmpty(department.DepartmentManagerId))
                throw new InvalidOperationException("This department has no manager assigned.");

            // Fetch manager by DepartmentManagerId
            var manager = await _employeeRepository.GetByIdAsync(department.DepartmentManagerId);

            if (manager == null)
                throw new ArgumentNullException(nameof(manager), "Manager not found.");

            // Map to response DTO
            return _mapper.Map<EmployeeResponseDto>(manager);
        }
        #endregion
            
        //#region Get Department Salary
        //public async Task<decimal> GetDepartmentTotalSalaryAsync(string departmentId)
        //{
        //    return await _departmentRepository.GetDepartmentTotalSalaryAsync(departmentId);
        //}
        //#endregion
    }
}
