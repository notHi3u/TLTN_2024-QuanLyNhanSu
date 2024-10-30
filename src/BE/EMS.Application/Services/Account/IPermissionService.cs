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
    public interface IPermissionService
    {
        Task<PermissionResponseDto> GetPermissionByIdAsync(string id);
        Task<PermissionResponseDto> CreatePermissionAsync(PermissionRequestDto permissionRequestDto);
        Task<PermissionResponseDto> UpdatePermissionAsync(string id, PermissionRequestDto permissionRequestDto);
        Task<bool> DeletePermissionAsync(string id);
        Task<PagedDto<PermissionResponseDto>> GetPagedPermissionsAsync(PermissionFilter filter);
    }
}
