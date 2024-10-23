using Common.Data;
using EMS.Domain.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Repositories.Account
{
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        Task<List<UserRole>> GetByRoleIdAsync(string roleId);
        Task<List<UserRole>> GetByUserIdAsync(string userId);
        Task<UserRole?> GetByUserAndRoleIdAsync(string userId, string roleId);
    }
}
