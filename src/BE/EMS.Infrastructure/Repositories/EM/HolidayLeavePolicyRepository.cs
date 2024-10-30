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
    public class HolidayLeavePolicyRepository : BaseRepository<HolidayLeavePolicy>, IHolidayLeavePolicyRepository
    {
        public HolidayLeavePolicyRepository(AppDbContext context, ILogger<HolidayLeavePolicyRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<PagedDto<HolidayLeavePolicy>> GetPagedAsync(HolidayLeavePolicyFilter filter)
        {
            // Initialize default values for the filter if necessary
            filter.PageIndex ??= 1;
            filter.PageSize ??= 10;

            var query = _dbSet.AsQueryable();

            // Apply filtering based on the filter properties
            if (filter.EffectiveYear != null)
            {
                query = query.Where(h => h.EffectiveYear == filter.EffectiveYear);
            }

            // Count total records for pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((filter.PageIndex.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value)
                .ToListAsync();

            return new PagedDto<HolidayLeavePolicy>(items, totalCount, filter.PageIndex.Value, filter.PageSize.Value);
        }
    }
}
