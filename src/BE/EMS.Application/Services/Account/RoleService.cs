using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.Account;
using EMS.Domain.Filters.Account;
using EMS.Domain.Models.Account;
using EMS.Domain.Repositories.Account;
using EMS.Infrastructure.Repositories.Account;
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
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUserRoleRepository _userRoleRepository;

        public RoleService(IRoleRepository roleRepository, IMapper mapper, IRolePermissionRepository rolePermissionRepository, UserManager<User> userManager, IUserRoleRepository userRoleRepository)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager;
            _rolePermissionRepository = rolePermissionRepository ?? throw new ArgumentNullException(nameof(_rolePermissionRepository));
            _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(_userRoleRepository));
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

            // Check if a role with the same name already exists using ExistsAsync
            var roledb = await _roleRepository.GetByIdAsync(roleRequestDto.Id);

            if (roledb != null)
            {
                throw new InvalidOperationException($"Role with name '{roleRequestDto.Name}' already exists.");
            }

            // Map the DTO to the entity and set the normalized name
            var role = _mapper.Map<Role>(roleRequestDto);
            role.NormalizedName = role.Name.ToUpper();
            role.Id = Guid.NewGuid().ToString();

            // Add the new role to the repository
            await _roleRepository.AddAsync(role);

            //Add RolePermission
            foreach(var permission in roleRequestDto.PermissionsIds)
            {

            }

            // Return the mapped RoleResponseDto
            return _mapper.Map<RoleResponseDto>(role);
        }

        public async Task<RoleResponseDto> UpdateRoleAsync(RoleRequestDto roleRequestDto)
        {
            if (roleRequestDto == null)
            {
                throw new ArgumentNullException(nameof(roleRequestDto));
            }

            var role = await _roleRepository.GetByIdAsync(roleRequestDto.Id);
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            // Handle Permissions
            var currentPermissions = await _rolePermissionRepository.GetByRoleIdAsync(roleRequestDto.Id);
            var currentPermissionIds = currentPermissions.Select(rp => rp.PermissionId).ToHashSet();

            // Add new permissions
            foreach (var permissionId in roleRequestDto.PermissionsIds)
            {
                if (!currentPermissionIds.Contains(permissionId))
                {
                    var newRolePermission = new RolePermission
                    {
                        RoleId = roleRequestDto.Id,
                        PermissionId = permissionId
                    };
                    await _rolePermissionRepository.AddAsync(newRolePermission);
                }
            }

            // Remove permissions not in the new list
            foreach (var permission in currentPermissions)
            {
                if (!roleRequestDto.PermissionsIds.Contains(permission.PermissionId))
                {
                    await _rolePermissionRepository.DeleteAsync(permission);
                }
            }

            // Update the role's basic details
            _mapper.Map(roleRequestDto, role);
            await _roleRepository.UpdateAsync(role);

            // Return the updated role as a response DTO
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

            // Check if the role exists
            var roleExists = await _roleRepository.GetByIdAsync(roleId);
            if (roleExists == null)
            {
                return BaseResponse<bool>.Failure("Role not found.");
            }

            var errors = new List<string>();

            foreach (var userId in userIds)
            {
                // Check if the user exists
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    errors.Add($"User with ID '{userId}' not found.");
                    continue; // Skip to the next user
                }

                // Check if the user already has the role
                var existingUserRole = await _userRoleRepository.GetByUserAndRoleIdAsync(userId, roleId);
                if (existingUserRole != null)
                {
                    continue; // User already has the role, skip
                }

                // Add the role to the user
                var newUserRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                };

                try
                {
                    await _userRoleRepository.AddAsync(newUserRole);
                }
                catch (Exception ex)
                {
                    errors.Add($"Failed to assign role to user '{userId}': {ex.Message}");
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
            var role = await _roleRepository.GetByIdAsync(roleId);
            if (role == null)
                throw new ArgumentNullException(nameof(role));
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            return _mapper.Map <IEnumerable<UserResponseDto>> (usersInRole);
        }

        public async Task<bool> RemoveUsersFromRoleAsync(string roleId, IEnumerable<string> userIds)
        {
            var role = await _roleRepository.GetByIdAsync(roleId);
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
