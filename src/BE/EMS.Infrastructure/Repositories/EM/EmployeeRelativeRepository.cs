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
    public class EmployeeRelativeRepository : BaseRepository<EmployeeRelative>, IEmployeeRelativeRepository
    {
        public EmployeeRelativeRepository(EMSDbContext context, ILogger<EmployeeRelativeRepository> logger)
            : base(context, logger)
        {
        }

        // Method to get paged employee relatives based on a filter
        public async Task<PagedDto<EmployeeRelative>> GetPagedAsync(EmployeeRelativeFilter filter)
        {
            // Initialize filter defaults
            filter.PageIndex = filter.PageIndex ?? 1;
            filter.PageSize = filter.PageSize ?? 10;

            var query = _dbSet.AsQueryable();

            // Optional filtering based on EmployeeId
            if (!string.IsNullOrWhiteSpace(filter.EmployeeId))
            {
                query = query.Where(er => er.EmployeeId == filter.EmployeeId);
            }

            // Apply sorting if specified
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

            return new PagedDto<EmployeeRelative>(items, totalCount, filter.PageIndex.Value, filter.PageSize.Value);
        }
    }
}
