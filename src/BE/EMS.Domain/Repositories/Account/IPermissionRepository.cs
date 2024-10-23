using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.Account;
using EMS.Domain.Models.Account;

namespace EMS.Domain.Repositories.Account
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        Task<Permission?> GetByIdAsync(string id);
        Task<PagedDto<Permission>> GetPagedAsync(PermissionFilter filter);
    }

}
