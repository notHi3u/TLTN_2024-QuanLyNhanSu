using Account.Domain.Filters;
using Common.Data;
using Common.Dtos;
using EMS.Domain.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account.Domain.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role?> GetByIdAsync(string id, bool IsDeep);
        Task<PagedDto<Role>> GetPagedAsync(RoleFilter filter);
    }
}
