using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.Account;
using EMS.Domain.Models.Account;
using EMS.Domain.Repositories.Account;
using EMS.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EMS.Infrastructure.Repositories.Account
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
            : base(context, logger)
        {
        }

        public async Task<PagedDto<User>> GetPagedAsync(UserFilter filter)
        {
            // Khởi tạo giá trị mặc định cho filter nếu cần
            filter.PageIndex = filter.PageIndex ?? 1;
            filter.PageSize = filter.PageSize ?? 10;

            var query = _dbSet.AsQueryable();

            // Nạp dữ liệu liên quan nếu IsDeep có giá trị
            if (filter.IsDeep.HasValue && filter.IsDeep.Value)
            {
                query = query
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission);
            }

            // Áp dụng bộ lọc từ khóa
            if (!string.IsNullOrWhiteSpace(filter.Keyword))
            {
                query = query.Where(u => u.UserName.Contains(filter.Keyword));
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

            return new PagedDto<User>(items, totalCount, filter.PageIndex.Value, filter.PageSize.Value);
        }

        public async Task<User> GetByIdAsync(string id, bool? isDeep = false)
        {
            var query = _dbSet.AsQueryable();

            // Fetch the user by Id
            var userQuery = query.Where(u => u.Id == id);

            // Include related data if isDeep is true
            if (isDeep.HasValue && isDeep.Value)
            {
                userQuery = userQuery
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.RolePermissions) // Include related RolePermissions
                    .ThenInclude(rp => rp.Permission);  // Include related Permissions
            }

            // Retrieve the user (either with or without related data)
            var user = await userQuery.FirstOrDefaultAsync();

            return user;
        }

    }
}
