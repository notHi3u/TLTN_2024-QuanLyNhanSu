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
    public class DepartmentRepository : BaseRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(AppDbContext context, ILogger<DepartmentRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<PagedDto<Department>> GetPagedAsync(DepartmentFilter filter)
        {
            // Initialize filter defaults
            filter.PageIndex = filter.PageIndex ?? 1;
            filter.PageSize = filter.PageSize ?? 10;

            var query = _dbSet.AsQueryable();

            if (filter.IsDeep.HasValue && filter.IsDeep.Value)
            {
                query = query
                    .Include(d => d.Employees)
                    .Include(d => d.Manager);
            }

            // Apply filtering based on department name or any other property in filter
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                query = query.Where(d => d.DepartmentName.Contains(filter.Keyword));
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

            return new PagedDto<Department>(items, totalCount, filter.PageIndex.Value, filter.PageSize.Value);
        }

        public async Task<Department> GetByIdAsync(string id, bool? isDeep = false)
        {
            var query = _dbSet.AsQueryable();

            // Include related data if isDeep is true
            if (isDeep.HasValue && isDeep.Value)
            {
                query = query
                    .Include(d => d.Employees)
                    .Include(d => d.Manager);
            }

            // Retrieve the department by ID
            var department = await query.FirstOrDefaultAsync(d => d.Id == id);

            return department;
        }

    }
}
