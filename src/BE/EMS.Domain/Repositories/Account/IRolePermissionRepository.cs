using Account.Domain.Models;
using Common.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.Domain.Repositories.Account
{
    public interface IRolePermissionRepository : IBaseRepository<RolePermission>
    {
        Task<List<RolePermission>> GetByRoleIdAsync(string roleId);
        Task<List<RolePermission>> GetByPermissionIdAsync(string permissionId);
        Task<RolePermission?> GetByRoleAndPermissionIdAsync(string roleId, string permissionId);
        // Additional methods to retrieve all RolePermissions by RoleId or PermissionId
        Task<List<RolePermission>> GetAllByRoleIdAsync(string roleId);
        Task<List<RolePermission>> GetAllByPermissionIdAsync(string permissionId);
    }
}
