using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;
using EMS.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EMS.Infrastructure.Repositories.EM
{
    public class SalaryRepository : BaseRepository<Salary>, ISalaryRepository
    {
        public SalaryRepository(AppDbContext context, ILogger<SalaryRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<PagedDto<Salary>> GetPagedAsync(SalaryFilter filter)
        {
            // Set default values for pagination
            filter.PageIndex ??= 1;
            filter.PageSize ??= 10;

            var query = _dbSet.AsQueryable();

            // Filter by EmployeeId if provided
            if (!string.IsNullOrWhiteSpace(filter.EmployeeId))
            {
                query = query.Where(s => s.EmployeeId == filter.EmployeeId);
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

            return new PagedDto<Salary>(items, totalCount, filter.PageIndex.Value, filter.PageSize.Value);
        }
    }
}
