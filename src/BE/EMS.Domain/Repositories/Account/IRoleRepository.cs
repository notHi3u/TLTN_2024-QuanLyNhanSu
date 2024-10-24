using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.Account;
using EMS.Domain.Models.Account;

namespace EMS.Domain.Repositories.Account
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<PagedDto<Role>> GetPagedAsync(RoleFilter filter);
    }
}
