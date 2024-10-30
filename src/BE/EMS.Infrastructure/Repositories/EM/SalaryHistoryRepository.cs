using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;
using EMS.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EMS.Infrastructure.Repositories.EM
{
    public class SalaryHistoryRepository : BaseRepository<SalaryHistory>, ISalaryHistoryRepository
    {
        public SalaryHistoryRepository(AppDbContext context, ILogger<SalaryHistoryRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<PagedDto<SalaryHistory>> GetPagedAsync(SalaryHistoryFilter filter)
        {
            // Initialize default values for the filter if necessary
            filter.PageIndex ??= 1;
            filter.PageSize ??= 10;

            var query = _dbSet.AsQueryable();

            // Apply filtering based on filter properties
            if (!string.IsNullOrWhiteSpace(filter.EmployeeId))
            {
                query = query.Where(sh => sh.EmployeeId == filter.EmployeeId);
            }

            if (filter.Year.HasValue)
            {
                query = query.Where(sh => sh.Year == filter.Year.Value);
            }

            if (filter.Month.HasValue)
            {
                query = query.Where(sh => sh.Month == filter.Month.Value);
            }

            // Count total records for pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((filter.PageIndex.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value)
                .ToListAsync();

            return new PagedDto<SalaryHistory>(items, totalCount, filter.PageIndex.Value, filter.PageSize.Value);
        }
    }
}
