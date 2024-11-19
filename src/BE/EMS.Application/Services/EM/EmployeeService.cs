using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.Account;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.Account;
using EMS.Domain.Repositories.EM;

namespace EMS.Application.Services.EM
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IUserRepository userRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException( nameof(userRepository));
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

        public async Task<EmployeeResponseDto> BindUserToEmployeeAsync(string employeeId, string userId)
        {
            // Retrieve the Employee by employeeId
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new ArgumentNullException(nameof(employee), "Employee not found.");

            // Retrieve the User by userId
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentNullException(nameof(user), "User not found.");

            // Check if the employee is already linked to a user
            if (employee.UserId != null)
                throw new InvalidOperationException("Employee is already linked to a user.");

            try
            {
                // Bind User to Employee
                employee.UserId = user.Id;
                await _employeeRepository.UpdateAsync(employee);

                // Map to EmployeeResponseDto and return
                return _mapper.Map<EmployeeResponseDto>(employee);
            }
            catch (Exception ex)
            {
                // Log or handle exception if necessary
                throw new ApplicationException("An error occurred while binding user to employee.", ex);
            }
        }

        public async Task<EmployeeResponseDto> UnBindUserToEmployeeAsync(string employeeId)
        {
            // Retrieve the Employee by employeeId
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new ArgumentNullException(nameof(employee), "Employee not found.");

            // Check if the employee is linked to the provided user
            if (employee.UserId == null)
                throw new InvalidOperationException("The employee is not linked to the specified user.");

            try
            {
                // Unbind User from Employee
                employee.UserId = null;
                await _employeeRepository.UpdateAsync(employee);

                // Map to EmployeeResponseDto and return
                return _mapper.Map<EmployeeResponseDto>(employee);
            }
            catch (Exception ex)
            {
                // Log or handle exception if necessary
                throw new ApplicationException("An error occurred while unbinding user from employee.", ex);
            }
        }

    }
}
