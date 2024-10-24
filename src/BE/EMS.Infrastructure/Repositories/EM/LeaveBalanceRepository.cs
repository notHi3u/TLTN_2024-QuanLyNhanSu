using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;
using EMS.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMS.Infrastructure.Repositories.EM
{
    public class LeaveBalanceRepository : BaseRepository<LeaveBalance>, ILeaveBalanceRepository
    {
        public LeaveBalanceRepository(EMSDbContext context, ILogger<LeaveBalanceRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<PagedDto<LeaveBalance>> GetPagedAsync(LeaveBalanceFilter filter)
        {
            // Initialize default values for the filter if necessary
            filter.PageIndex ??= 1;
            filter.PageSize ??= 10;

            var query = _dbSet.AsQueryable();

            // Apply filtering based on the filter properties
            if (!string.IsNullOrWhiteSpace(filter.EmployeeId))
            {
                query = query.Where(lb => lb.EmployeeId == filter.EmployeeId);
            }

            if (filter.Year != null)
            {
                query = query.Where(lb => lb.Year == filter.Year);
            }

            // Count total records for pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((filter.PageIndex.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value)
                .ToListAsync();

            return new PagedDto<LeaveBalance>(items, totalCount, filter.PageIndex.Value, filter.PageSize.Value);
        }
    }
}
