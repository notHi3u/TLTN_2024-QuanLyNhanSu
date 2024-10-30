using EMS.Application.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.Services.Account
{
    public interface IRolePermissionService
    {
        Task<RolePermissionResponseDto?> GetRolePermissionByRoleIdAsync(string roleId);
        Task<RolePermissionResponseDto?> GetRolePermissionByPermissionIdAsync(string permissionId);
        Task<List<RolePermissionResponseDto?>> GetRolePermissionsByRoleIdAsync(string roleId);
        Task<List<RolePermissionResponseDto?>> GetRolePermissionsByPermissionIdAsync(string permissionId);
        Task<RolePermissionResponseDto?> AddRolePermissionAsync(string roleId, string permissionId);
        Task<bool> RemoveRolePermissionAsync(string roleId, string permissionId);
        Task<bool> RolePermissionExistsAsync(string roleId, string permissionId);
    }
}
