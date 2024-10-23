using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;
using EMS.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMS.Infrastructure.Repositories.EM
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(EMSDbContext context, ILogger<EmployeeRepository> logger)
            : base(context, logger)
        {
        }

        // Get employee by ID
        public async Task<Employee?> GetByIdAsync(string id)
        {
            _logger.LogInformation($"Getting Employee with ID {id}");
            return await _dbSet
                .AsNoTracking() // Optimizes for read-only queries
                .FirstOrDefaultAsync(e => e.Id == id);
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

        // Add new employee
        public async Task AddEmployeeAsync(Employee employee)
        {
            _logger.LogInformation($"Adding Employee {employee.FirstName} {employee.LastName}");
            await _dbSet.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        // Update existing employee
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _logger.LogInformation($"Updating Employee {employee.Id}");
            _dbSet.Update(employee);
            await _context.SaveChangesAsync();
        }

        // Delete employee by ID
        public async Task DeleteEmployeeAsync(string id)
        {
            _logger.LogInformation($"Deleting Employee with ID {id}");
            var employee = await GetByIdAsync(id);
            if (employee != null)
            {
                _dbSet.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}
