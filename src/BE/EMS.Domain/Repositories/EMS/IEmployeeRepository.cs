using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Repositories.EMS
{
    namespace EMS.Domain.Repositories.EMS
    {
        public interface IEmployeeRepository : IBaseRepository<Employee>
        {
            Task<Employee?> GetByIdAsync(string id);
            Task<PagedDto<Employee>> GetPagedAsync(EmployeeFilter filter);
            Task AddEmployeeAsync(Employee employee);
            Task UpdateEmployeeAsync(Employee employee);
            Task DeleteEmployeeAsync(string id);
        }
    }
}
