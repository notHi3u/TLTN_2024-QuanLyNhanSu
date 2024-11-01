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
    public class AttendanceRepository : BaseRepository<Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(AppDbContext context, ILogger<AttendanceRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<PagedDto<Attendance>> GetPagedAsync(AttendanceFilter filter)
        {
            // Initialize filter defaults
            filter.PageIndex = filter.PageIndex ?? 1;
            filter.PageSize = filter.PageSize ?? 10;

            var query = _dbSet.AsQueryable();

            // Filter by employee ID if provided
            if (!string.IsNullOrWhiteSpace(filter.EmployeeId))
            {
                query = query.Where(a => a.EmployeeId == filter.EmployeeId);
            }

            if (filter.TimeCardId != null)
            {
                query = query.Where(a => a.TimeCardId == filter.TimeCardId);
            }

            // Filter by date range if provided
            if (filter.StartDate.HasValue)
            {
                query = query.Where(a => a.Date >= filter.StartDate.Value);
            }

            if (filter.EndDate.HasValue)
            {
                query = query.Where(a => a.Date <= filter.EndDate.Value);
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                query = filter.IsSortDescending.HasValue && filter.IsSortDescending.Value
                    ? query.OrderByDescending(e => EF.Property<object>(e, filter.SortBy))
                    : query.OrderBy(e => EF.Property<object>(e, filter.SortBy));
            }

            // Get total count for pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((filter.PageIndex.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value)
                .ToListAsync();

            return new PagedDto<Attendance>(items, totalCount, filter.PageIndex.Value, filter.PageSize.Value);
        }
    }
}
