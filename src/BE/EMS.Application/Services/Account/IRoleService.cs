﻿using Common.Dtos;
using EMS.Application.DTOs;
using EMS.Domain.Filters.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.Services.Account
{
    public interface IRoleService
    {
        Task<RoleResponseDto> GetRoleByIdAsync(string id, bool IsDeep);
        Task<RoleResponseDto> CreateRoleAsync(RoleRequestDto roleRequestDto);
        Task<RoleResponseDto> UpdateRoleAsync(string id, RoleRequestDto roleRequestDto);
        Task<bool> DeleteRoleAsync(string id);
        Task<PagedDto<RoleResponseDto>> GetPagedRolesAsync(RoleFilter filter);
    }
}
