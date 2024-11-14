using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;

namespace EMS.Application.Services.EM
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<EmployeeResponseDto> CreateEmployeeAsync(EmployeeRequestDto employeeRequestDto)
        {
            if (employeeRequestDto == null)
                throw new ArgumentNullException(nameof(employeeRequestDto));

            Employee employee = _mapper.Map<Employee>(employeeRequestDto);
            await _employeeRepository.AddAsync(employee);
            return _mapper.Map<EmployeeResponseDto>(employee);
        }

        public async Task<bool> DeleteEmployeeAsync(string id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            await _employeeRepository.DeleteAsync(employee);
            return true;
        }

        public async Task<EmployeeResponseDto> GetEmployeeByIdAsync(string id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            return _mapper.Map<EmployeeResponseDto>(employee);
        }

        public async Task<PagedDto<EmployeeResponseDto>> GetPagedEmployeesAsync(EmployeeFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedEmployees = await _employeeRepository.GetPagedAsync(filter);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeResponseDto>>(pagedEmployees.Items);

            return new PagedDto<EmployeeResponseDto>(
                employeeDtos,
                pagedEmployees.TotalCount,
                pagedEmployees.PageIndex,
                pagedEmployees.PageSize
            );
        }

        public async Task<EmployeeResponseDto> UpdateEmployeeAsync(string id, EmployeeRequestDto employeeRequestDto)
        {
            if (employeeRequestDto == null)
                throw new ArgumentNullException(nameof(employeeRequestDto));

            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            _mapper.Map(employeeRequestDto, employee);
            await _employeeRepository.UpdateAsync(employee);

            return _mapper.Map<EmployeeResponseDto>(employee);
        }

        public async Task<bool> AssignDepartmentAsync(string id, string departmentId)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            employee.DepartmentId = departmentId;
            try
            {
                await _employeeRepository.UpdateAsync(employee);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> RemoveDepartmentAsync(string id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            employee.DepartmentId = null;

            try
            {
                await _employeeRepository.UpdateAsync(employee);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
