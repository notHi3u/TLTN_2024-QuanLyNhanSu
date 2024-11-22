using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;



namespace EMS.Domain.Repositories.EM
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<PagedDto<Employee>> GetPagedAsync(EmployeeFilter filter);

        // Task to link employee to a user
        Task<bool> LinkEmployeeToUserAsync(string employeeId, string userId);
        Task<IEnumerable<Employee>> GetByDepartmentIdAsync(string departmentId);
        Task<decimal> GetTotalSalaryAsync();
        Task<Employee> GetEmployeeByUserIdAsync(string userId)
    }
}