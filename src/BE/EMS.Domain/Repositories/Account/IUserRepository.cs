using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.Account;
using EMS.Domain.Models.Account;

namespace EMS.Domain.Repositories.Account
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<PagedDto<User>> GetPagedAsync(UserFilter filter);
        Task<User> GetByIdAsync(string id, bool? isDeep);
    }
}
