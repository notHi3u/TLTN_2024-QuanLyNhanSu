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
    public class LeaveRequestRepository : BaseRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(EMSDbContext context, ILogger<LeaveRequestRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<PagedDto<LeaveRequest>> GetPagedAsync(LeaveRequestFilter filter)
        {
            // Initialize default values for the filter if necessary
            filter.PageIndex ??= 1;
            filter.PageSize ??= 10;

            var query = _dbSet.AsQueryable();

            // Apply filtering based on filter properties
            if (!string.IsNullOrWhiteSpace(filter.EmployeeId))
            {
                query = query.Where(lr => lr.EmployeeId == filter.EmployeeId);
            }

            if (!string.IsNullOrWhiteSpace(filter.LeaveType))
            {
                query = query.Where(lr => lr.LeaveType == filter.LeaveType);
            }

            if (filter.StartDate.HasValue)
            {
                query = query.Where(lr => lr.StartDate >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(lr => lr.EndDate <= filter.EndDate.Value);
            }

            // Count total records for pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((filter.PageIndex.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value)
                .ToListAsync();

            return new PagedDto<LeaveRequest>(items, totalCount, filter.PageIndex.Value, filter.PageSize.Value);
        }
    }
}
