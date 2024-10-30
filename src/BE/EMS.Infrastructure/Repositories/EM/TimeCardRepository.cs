using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.EMS; // Ensure you have a TimeCardFilter class
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;
using EMS.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Infrastructure.Repositories.EM
{
    public class TimeCardRepository : BaseRepository<TimeCard>, ITimeCardRepository
    {
        public TimeCardRepository(AppDbContext context, ILogger<TimeCardRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<PagedDto<TimeCard>> GetPagedAsync(TimeCardFilter filter)
        {
            // Initialize default values for the filter if necessary
            filter.PageIndex ??= 1;
            filter.PageSize ??= 10;

            var query = _dbSet.AsQueryable()
                .Include(tc => tc.Attendances) // Include related Attendance records if necessary
                .Include(tc => tc.Employee); // Include related Employee if necessary

            // Apply filtering based on filter properties
            if (!string.IsNullOrWhiteSpace(filter.EmployeeId))
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TimeCard, Employee>)query.Where(tc => tc.EmployeeId == filter.EmployeeId);
            }

            if (filter.WeekStartDate != null)
            {
                query = (Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TimeCard, Employee>)query.Where(tc => tc.WeekStartDate == filter.WeekStartDate);
            }

            // Count total records for pagination
            var totalCount = await query.CountAsync();

            // Apply pagination
            var items = await query
                .Skip((filter.PageIndex.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value)
                .ToListAsync();

            return new PagedDto<TimeCard>(items, totalCount, filter.PageIndex.Value, filter.PageSize.Value);
        }
    }
}
