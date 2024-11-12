using Common.Dtos;
using EMS.Application.DTOs.Account;
using EMS.Domain.Filters.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.Services.Account
{
    public interface IUserService
    {
        Task<BaseResponse<UserResponseDto>> GetUserByIdAsync(string id, bool? isDeep);
        Task<BaseResponse<UserResponseDto>> CreateUserAsync(UserRequestDto userRequestDto);
        Task<BaseResponse<UserResponseDto>> UpdateUserAsync(string id, UserRequestDto userRequestDto);
        Task<BaseResponse<bool>> DeleteUserAsync(string id);
        Task<PagedDto<UserResponseDto>> GetPagedUsersAsync(UserFilter filter); // Đã điều chỉnh để sử dụng BaseResponse
        Task<BaseResponse<bool>> AssignRoleToUserAsync(string userId, string roleName);
        Task<BaseResponse<bool>> AssignRolesToUserAsync(string userId, IEnumerable<string> roleNames);

        Task<BaseResponse<bool>> RemoveRoleFromUserAsync(string userId, string roleName);
        Task<BaseResponse<bool>> RemoveRolesFromUserAsync(string userid, IEnumerable<string> roleNames);
        Task<BaseResponse<bool>> UpdateUserRolesAsync(string userId, IEnumerable<string> roleNames);
    }
}
