using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.Account;
using EMS.Domain.Models.Account;
using EMS.Domain.Repositories.Account;
using EMS.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EMS.Infrastructure.Repositories.Account
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context, ILogger<RoleRepository> logger, UserManager<User> userManager)
            : base(context, logger)
        {
        }

        public async Task<Role> GetByIdAsync(string id, bool? isDeep = false)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id), "ID cannot be null or empty.");

            var query = _dbSet.AsQueryable();

            if (isDeep.HasValue && isDeep.Value)
            {
                // Include related entities for deep fetching
                query = query.Include(r => r.RolePermissions)
                             .ThenInclude(rp => rp.Permission);
            }
            else
            {
                // Use AsNoTracking for lightweight fetching
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(r => r.Id == id);
        }


        public async Task<IEnumerable<string>> GetIdByNameAsync(IEnumerable<string> names)
        {
            if (names == null || !names.Any())
                throw new ArgumentNullException(nameof(names), "Names cannot be null or empty.");

            return await _dbSet
                .Where(r => names.Contains(r.Name)) // Filter roles by the provided names
                .Select(r => r.Id)                 // Project only the IDs
                .ToListAsync();                    // Execute the query and return the results as a list
        }


        public async Task<PagedDto<Role>> GetPagedAsync(RoleFilter filter)
        {
            // Đảm bảo filter không phải là null và khởi tạo giá trị mặc định nếu cần
            filter.PageIndex = filter.PageIndex ?? 1;
            filter.PageSize = filter.PageSize ?? 10;

            var query = _dbSet.AsQueryable();

            // Nạp dữ liệu liên quan nếu IsDeep là true
            if (filter.IsDeep.HasValue && filter.IsDeep.Value)
            {
                query = query
                    .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission);
            }

            // Áp dụng bộ lọc từ khóa
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                query = query.Where(r => r.Name.Contains(filter.Keyword));
            }

            // Áp dụng sắp xếp
            if (!string.IsNullOrWhiteSpace(filter.SortBy))
            {
                query = filter.IsSortDescending.HasValue && filter.IsSortDescending.Value
                    ? query.OrderByDescending(e => EF.Property<object>(e, filter.SortBy))
                    : query.OrderBy(e => EF.Property<object>(e, filter.SortBy));
            }

            // Lấy tổng số bản ghi để phân trang
            var totalCount = await query.CountAsync();

            // Áp dụng phân trang
            var items = await query
                .Skip((filter.PageIndex.Value - 1) * filter.PageSize.Value)
                .Take(filter.PageSize.Value)
                .ToListAsync();

            return new PagedDto<Role>(items, totalCount, filter.PageIndex.Value, filter.PageSize.Value);
        }
    }
}
