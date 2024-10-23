using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;



namespace EMS.Domain.Repositories.EM
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        Task<Employee?> GetByIdAsync(string id);
        Task<PagedDto<Employee>> GetPagedAsync(EmployeeFilter filter);
    }
}