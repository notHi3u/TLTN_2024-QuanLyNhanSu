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
        private readonly ISalaryRecordRepository _salaryRecordRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IUserRepository userRepository, ISalaryRecordRepository salaryRecordRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userRepository = userRepository ?? throw new ArgumentNullException( nameof(userRepository));
            _salaryRecordRepository = salaryRecordRepository ?? throw new ArgumentNullException(nameof(salaryRecordRepository));
        }

        public async Task<EmployeeResponseDto> CreateEmployeeAsync(EmployeeRequestDto employeeRequestDto)
        {
            if (employeeRequestDto == null)
                throw new ArgumentNullException(nameof(employeeRequestDto));

            Employee employee = _mapper.Map<Employee>(employeeRequestDto);
            employee.Id = Guid.NewGuid().ToString();
            await _employeeRepository.AddAsync(employee);

            //var initSalaryRecord = _mapper.Map<SalaryRecord>(employee);
            var initSalaryRecord = new SalaryRecord()
            { EmployeeId = employee.Id, BaseSalary = employee.BaseSalary, Bonuses = employee.Bonuses, Deductions = employee.Deductions };
            initSalaryRecord.Year = DateTime.Now.Year;
            initSalaryRecord.Month = DateTime.Now.Month;
            await _salaryRecordRepository.AddAsync(initSalaryRecord);

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

            // Validation for positive values
            if (employeeRequestDto.BaseSalary <= 0)
                throw new ArgumentException("Base salary must be a positive value.", nameof(employeeRequestDto.BaseSalary));

            if (employeeRequestDto.Bonuses < 0)
                throw new ArgumentException("Bonuses cannot be negative.", nameof(employeeRequestDto.Bonuses));

            if (employeeRequestDto.Deductions < 0)
                throw new ArgumentException("Deductions cannot be negative.", nameof(employeeRequestDto.Deductions));

            // Check for changes in BaseSalary
            bool hasSalaryRelatedChanges =
                employeeRequestDto.BaseSalary != employee.BaseSalary ||
                employeeRequestDto.Bonuses != employee.Bonuses ||
                employeeRequestDto.Deductions != employee.Deductions;

            // Map updated fields
            _mapper.Map(employeeRequestDto, employee);

            // Update employee record
            await _employeeRepository.UpdateAsync(employee);

            // Add new salary record if salary has changed
            if (hasSalaryRelatedChanges)
            {
                var newSalaryRecord = new SalaryRecord
                {
                    EmployeeId = employee.Id ?? id,
                    BaseSalary = employee.BaseSalary,
                    Bonuses = employee.Bonuses,
                    Deductions = employee.Deductions,
                    Year = DateTime.Now.Year,
                    Month = DateTime.Now.Month
                };

                await _salaryRecordRepository.AddAsync(newSalaryRecord);
            }

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

        public async Task<decimal> GetTotalSalaryAsync()
        {
            return await _employeeRepository.GetTotalSalaryAsync();
        }

        public async Task<string> SaveEmployeeImage(EmployeeImageDto employeeImageDto)
        {
            // Fetch the employee from the repository
            var employee = await _employeeRepository.GetByIdAsync(employeeImageDto.EmployeeId);
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), $"Employee with ID {employeeImageDto.EmployeeId} not found.");
            }

            // Check if an image is provided
            if (employeeImageDto.Image == null || employeeImageDto.Image.Length == 0)
            {
                throw new ArgumentException("No image file provided.", nameof(employeeImageDto.Image));
            }

            // If the employee already has an image, delete the old one
            if (!string.IsNullOrEmpty(employee.ImageUrl))
            {
                var existingImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", employee.ImageUrl.TrimStart('/'));
                if (File.Exists(existingImagePath))
                {
                    // Delete the existing image file
                    File.Delete(existingImagePath);
                }
            }

            // Generate a unique file name for the new image
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(employeeImageDto.Image.FileName);

            // Define the path to save the new image (e.g., in wwwroot/Images)
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

            // Ensure the directory exists
            var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Save the new image to the file system
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await employeeImageDto.Image.CopyToAsync(stream);
            }

            // Update the employee's image URL with the relative path
            employee.ImageUrl = $"/Images/{fileName}";

            // Save the new image URL in the database using the repository
            var url = await _employeeRepository.SaveImageUrl(employee.ImageUrl, employee.Id);

            // Return the updated URL to the image (or a confirmation message)
            return url;
        }


    }
}
