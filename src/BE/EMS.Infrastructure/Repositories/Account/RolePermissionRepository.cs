using Common.Data;
using EMS.Domain.Models.Account;
using EMS.Domain.Repositories.Account;
using EMS.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMS.Infrastructure.Repositories.Account
{
    public class RolePermissionRepository : BaseRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(AppDbContext context, ILogger<RolePermissionRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<List<RolePermission>> GetByRoleIdAsync(string roleId)
        {
            _logger.LogInformation($"Getting RolePermissions with Role ID {roleId}");
            return await _dbSet
                .AsNoTracking() // Improves performance for read-only queries
                .Where(rp => rp.RoleId == roleId) // Fetch all RolePermissions with the specified RoleId
                .ToListAsync(); // Use ToListAsync to get a list
        }

        public async Task<List<RolePermission>> GetByPermissionIdAsync(string permissionId)
        {
            _logger.LogInformation($"Getting RolePermissions with Permission ID {permissionId}");
            return await _dbSet
                .AsNoTracking() // Improves performance for read-only queries
                .Where(rp => rp.PermissionId == permissionId) // Fetch all RolePermissions with the specified PermissionId
                .ToListAsync(); // Use ToListAsync to get a list
        }

        public async Task<RolePermission?> GetByRoleAndPermissionIdAsync(string roleId, string permissionId)
        {
            _logger.LogInformation($"Getting RolePermission with Role ID {roleId} and Permission ID {permissionId}");
            return await _dbSet
                .AsNoTracking() // Improves performance for read-only queries
                .FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId); // Fetch the RolePermission with both RoleId and PermissionId
        }
    }
}
