using Account.Domain.Models;
using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Repositories.Account
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        Task<Permission?> GetByIdAsync(string id);
        Task<PagedDto<Permission>> GetPagedAsync(PermissionFilter filter);
    }

}
