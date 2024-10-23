using Common.Data;
using EMS.Domain.Models.Account;

namespace EMS.Domain.Repositories.Account
{
    public interface IRolePermissionRepository : IBaseRepository<RolePermission>
    {
        Task<List<RolePermission>> GetByRoleIdAsync(string roleId);
        Task<List<RolePermission>> GetByPermissionIdAsync(string permissionId);
        Task<RolePermission?> GetByRoleAndPermissionIdAsync(string roleId, string permissionId);
    }
}
