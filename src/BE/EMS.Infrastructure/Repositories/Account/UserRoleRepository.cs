using Common.Data;
using EMS.Domain.Models.Account;
using EMS.Domain.Repositories.Account;
using EMS.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMS.Infrastructure.Repositories.Account
{
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(AppDbContext context, ILogger<UserRoleRepository> logger)
            : base(context, logger)
        {
        }

        // Get all UserRoles by RoleId
        public async Task<List<UserRole>> GetByRoleIdAsync(string roleId)
        {
            _logger.LogInformation($"Getting UserRoles with Role ID {roleId}");
            return await _dbSet
                .AsNoTracking() // For read-only queries, no need to track changes
                .Where(ur => ur.RoleId == roleId)
                .ToListAsync(); // Fetch all UserRoles with the specified RoleId
        }

        // Get all UserRoles by UserId
        public async Task<List<UserRole>> GetByUserIdAsync(string userId)
        {
            _logger.LogInformation($"Getting UserRoles with User ID {userId}");
            return await _dbSet
                .AsNoTracking() // For read-only queries, no need to track changes
                .Where(ur => ur.UserId == userId)
                .ToListAsync(); // Fetch all UserRoles with the specified UserId
        }

        // Get a specific UserRole by UserId and RoleId
        public async Task<UserRole?> GetByUserAndRoleIdAsync(string userId, string roleId)
        {
            _logger.LogInformation($"Getting UserRole with User ID {userId} and Role ID {roleId}");
            return await _dbSet
                .AsNoTracking() // For read-only queries, no need to track changes
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId); // Fetch the UserRole that matches both UserId and RoleId
        }
    }
}
