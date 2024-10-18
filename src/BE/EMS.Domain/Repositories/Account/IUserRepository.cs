using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.Account;
using EMS.Domain.Models.Account;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Domain.Repositories.Account
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> GetByIdAsync(string id);
        Task<PagedDto<User>> GetPagedAsync(UserFilter filter);
    }
}
