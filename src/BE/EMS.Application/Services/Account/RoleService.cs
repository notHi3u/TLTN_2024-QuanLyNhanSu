using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.Account;
using EMS.Domain.Filters.Account;
using EMS.Domain.Models.Account;
using EMS.Domain.Repositories.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.Services.Account
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleService(IRoleRepository roleRepository, IMapper mapper, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager;
        }

        public async Task<RoleResponseDto> GetRoleByIdAsync(string id, bool IsDeep)
        {
            var role = await _roleRepository.GetByIdAsync(id, IsDeep);
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<RoleResponseDto> CreateRoleAsync(RoleRequestDto roleRequestDto)
        {
            if (roleRequestDto == null)
            {
                throw new ArgumentNullException(nameof(roleRequestDto));
            }

            var role = _mapper.Map<Role>(roleRequestDto);
            role.NormalizedName = role.Name.ToUpper();
            role.Id = Guid.NewGuid().ToString();
            await _roleRepository.AddAsync(role);
            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<RoleResponseDto> UpdateRoleAsync(string id, RoleRequestDto roleRequestDto)
        {
            if (roleRequestDto == null)
            {
                throw new ArgumentNullException(nameof(roleRequestDto));
            }

            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            _mapper.Map(roleRequestDto, role);
            await _roleRepository.UpdateAsync(role);
            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<bool> DeleteRoleAsync(string id)
        {
            var role = await _roleRepository.GetByIdAsync(id);
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            await _roleRepository.DeleteAsync(role);
            return true;
        }

        public async Task<PagedDto<RoleResponseDto>> GetPagedRolesAsync(RoleFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var pagedRoles = await _roleRepository.GetPagedAsync(filter);

            var roleResponseDtos = _mapper.Map<IEnumerable<RoleResponseDto>>(pagedRoles.Items);

            var pagedResponse = new PagedDto<RoleResponseDto>(
                roleResponseDtos,
                pagedRoles.TotalCount,
                pagedRoles.PageIndex,
                pagedRoles.PageSize
            );

            return pagedResponse;
        }

        public async Task<BaseResponse<bool>> AssignRoleToMultipleUsersAsync(string roleId, IEnumerable<string> userIds)
        {
            if (string.IsNullOrWhiteSpace(roleId) || userIds == null || !userIds.Any())
            {
                return BaseResponse<bool>.Failure("Role ID cannot be null or empty, and at least one user ID must be provided.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleId);
            if (!roleExists)
            {
                return BaseResponse<bool>.Failure("Role not found.");
            }

            var errors = new List<string>();

            foreach (var userId in userIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    errors.Add($"User with ID '{userId}' not found.");
                    continue; // Skip to the next user
                }

                var result = await _userManager.AddToRoleAsync(user, roleId);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors.Select(e => e.Description));
                }
            }

            if (errors.Any())
            {
                return BaseResponse<bool>.Failure(errors);
            }

            return BaseResponse<bool>.Success(true);
        }

        public async Task<IEnumerable<UserResponseDto>> GetUsersByRoleIdAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            return _mapper.Map <IEnumerable<UserResponseDto>> (usersInRole);
        }

        public async Task<bool> RemoveUsersFromRoleAsync(string roleId, IEnumerable<string> userIds)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            foreach (var userId in userIds)
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }
            return true; // Indicate success
        }

    }
}
