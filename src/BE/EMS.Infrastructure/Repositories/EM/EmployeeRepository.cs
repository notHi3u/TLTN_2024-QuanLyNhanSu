using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.Account;
using EMS.Domain.Repositories.EM;
using EMS.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMS.Infrastructure.Repositories.EM
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        private readonly IUserRepository _userRepository;
        public EmployeeRepository(AppDbContext context, ILogger<EmployeeRepository> logger, IUserRepository userRepository)
            : base(context, logger)
        {
            _userRepository = userRepository;
        }
        

        // Get employees with pagination
        public async Task<PagedDto<Employee>> GetPagedAsync(EmployeeFilter filter)
        {
            _logger.LogInformation("Getting paged Employees");

            filter ??= new EmployeeFilter();

            int pageIndex = filter.PageIndex ?? 1;
            int pageSize = filter.PageSize ?? 10;

            if (pageIndex <= 0) pageIndex = 1;
            if (pageSize <= 0) pageSize = 10;

            var query = _dbSet.AsQueryable();

            // Apply filtering based on Keyword
            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                query = query.Where(e => e.FirstName!.Contains(filter.Keyword) || e.LastName!.Contains(filter.Keyword));
            }

            // Calculate total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var employees = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedDto<Employee>(employees, totalCount, pageIndex, pageSize);
        }

        public async Task<bool> LinkEmployeeToUserAsync(string employeeId, string userId)
        {
            var employee = await GetByIdAsync(userId);

            var user = await _userRepository.GetByIdAsync(userId);

            if (employee != null && user !=null)
            {
                employee.UserId = user.Id;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Employee>> GetByDepartmentIdAsync(string departmentId)
        {
            // Query the Employees table to get employees that belong to the given departmentId
            var employees = await _dbSet
                .Where(e => e.DepartmentId == departmentId) // Filter by departmentId
                .ToListAsync(); // Execute the query asynchronously and return a list of employees

            return employees;
        }

        public async Task<decimal> GetTotalSalaryAsync()
        {
            return await _dbSet.SumAsync(e => e.NetSalary);
        }

        public async Task<Employee> GetEmployeeByUserIdAsync(string userId)
        {
            var employee = await _dbSet.FirstOrDefaultAsync(e => e.UserId == userId);
            return employee;
        }

        public async Task<string> SaveImageUrl(string imgUrl, string employeeId)
        {
            var employee = await _dbSet.FirstOrDefaultAsync(e => e.Id == employeeId);
            if (employee != null)
            {
                employee.ImageUrl = imgUrl;
                await _context.SaveChangesAsync();
                return employee.ImageUrl;
            }
            return "";
        }
    }
}
