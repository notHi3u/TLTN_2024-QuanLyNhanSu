using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.Account;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.Account;
using EMS.Domain.Filters.Account;
using EMS.Domain.Models.Account;
using EMS.Domain.Models.EM;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EMS.API.Endpoints.Account
{
    public static class RoleEndpoints
    {
        public static void Map(WebApplication app)
        {
            #region Define
            var roleGroup = app.MapGroup("/roles")
                .WithTags("Role")
                .RequireAuthorization();
            #endregion

            #region Get all roles
            // Endpoint to get paged roles
            roleGroup.MapGet("/", async (IRoleService roleService, [FromBody] RoleFilter filter) =>
            {
                try
                {
                    var pagedRoles = await roleService.GetPagedRolesAsync(filter);
                    return Results.Ok(BaseResponse<PagedDto<RoleResponseDto>>.Success(pagedRoles));
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(BaseResponse<PagedDto<RoleResponseDto>>.Failure(ex.Message, 400));
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get role by Id
            // Endpoint to get role by ID
            roleGroup.MapGet("/{id}", async (IRoleService roleService, string id, bool? IsDeep = false) =>
            {
                var role = await roleService.GetRoleByIdAsync(id, (bool)IsDeep);
                if (role == null)
                {
                    return Results.NotFound(BaseResponse<RoleResponseDto>.Failure("Role not found", 404));
                }
                return Results.Ok(BaseResponse<RoleResponseDto>.Success(role));
            }).ConfigureApiResponses();
            #endregion

            #region Create Role
            // Endpoint to create a new role
            roleGroup.MapPost("/", async (IRoleService roleService, [FromBody] RoleRequestDto createRoleDto) =>
            {
                if (createRoleDto == null)
                {
                    return Results.BadRequest(BaseResponse<RoleResponseDto>.Failure("Role data is required.", 400));
                }

                var role = await roleService.CreateRoleAsync(createRoleDto);
                return Results.Ok(BaseResponse<RoleResponseDto>.Success(role));
            }).ConfigureApiResponses();
            #endregion

            #region Update role
            // Endpoint to update a role by ID
            roleGroup.MapPut("/{id}", async (IRoleService roleService, [FromBody] RoleRequestDto updateRoleDto) =>
            {
                if (updateRoleDto == null)
                {
                    return Results.BadRequest(BaseResponse<RoleResponseDto>.Failure("Role data is required.", 400));
                }

                var updatedRole = await roleService.UpdateRoleAsync(updateRoleDto);
                if (updatedRole == null)
                {
                    return Results.NotFound(BaseResponse<RoleResponseDto>.Failure("Role not found", 404));
                }
                return Results.Ok(BaseResponse<RoleResponseDto>.Success(updatedRole));
            }).ConfigureApiResponses();
            #endregion

            #region Delete role
            // Endpoint to delete a role by ID
            roleGroup.MapDelete("/{id}", async (IRoleService roleService, string id) =>
            {
                var success = await roleService.DeleteRoleAsync(id);
                if (!success)
                {
                    return Results.NotFound(BaseResponse<bool>.Failure("Role not found", 404));
                }
                return Results.BadRequest(BaseResponse<bool>.Failure(id,400));
            }).ConfigureApiResponses();
            #endregion

            #region Assign Permission
            roleGroup.MapPost("/{roleid}/permission", async (IRolePermissionService rolePermissionService, string roleId, string permissisonId) =>
            {
                if (string.IsNullOrEmpty(permissisonId))
                    return Results.BadRequest(BaseResponse<RolePermissionResponseDto>.Failure("Permission ID is required.", 400));

                var newRolePermission = await rolePermissionService.AddRolePermissionAsync(roleId, permissisonId);
                if (newRolePermission == null)
                    return Results.NotFound(BaseResponse<RolePermissionResponseDto>.Failure("Permission assignment failed.", 404));

                return Results.Ok(BaseResponse<RolePermissionResponseDto>.Success(newRolePermission,200));
            }).ConfigureApiResponses();
            #endregion

            #region Revoke Permission
            roleGroup.MapDelete("/{roleid}/permission", async (IRolePermissionService rolePermissionService, string roleId, string permissisonId) =>
            {
                if (string.IsNullOrEmpty(permissisonId))
                    return Results.BadRequest(BaseResponse<bool>.Failure("Permission ID is required.", 400));

                var newRolePermission = await rolePermissionService.RemoveRolePermissionAsync(roleId, permissisonId);
                if (!newRolePermission)
                    return Results.NotFound(BaseResponse<bool>.Failure("Permission revocation failed.", 404));

                return Results.Ok(BaseResponse<bool>.Success(true,200));
            }).ConfigureApiResponses();
            #endregion

            #region Assign Role To Multiple Users
            roleGroup.MapPost("/{roleid}/users", async (IRoleService roleService, string roleId, [FromBody] IEnumerable<string> userIds) =>
            {
                try
                {
                    if (userIds == null || !userIds.Any())
                    {
                        return Results.BadRequest(BaseResponse<bool>.Failure("At least one user ID must be provided.", 400));
                    }

                    var response = await roleService.AssignRoleToMultipleUsersAsync(roleId, userIds);
                    if (!response.IsSuccess)
                    {
                        return Results.BadRequest(BaseResponse<bool>.Failure(response.Errors, 400));
                    }

                    return Results.Ok(BaseResponse<bool>.Success(true, 200));
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(BaseResponse<bool>.Failure(ex.Message, 500));
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get Users By Role
            // Endpoint to retrieve all users assigned to a specific role
            roleGroup.MapGet("/{roleid}/users", async (IRoleService roleService, string roleId) =>
            {
                try
                {
                    var users = await roleService.GetUsersByRoleIdAsync(roleId);
                    if (users == null || !users.Any())
                    {
                        return Results.NotFound(BaseResponse<IEnumerable<UserResponseDto>>.Failure("No users found for this role.", 404));
                    }
                    return Results.Ok(BaseResponse<IEnumerable<UserResponseDto>>.Success(users));
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(BaseResponse<IEnumerable<UserResponseDto>>.Failure(ex.Message, 500));
                }
            }).ConfigureApiResponses();
            #endregion

            #region Remove Multiple Users From Role
            // Endpoint to remove multiple users from a specific role
            roleGroup.MapDelete("/{roleid}/users", async (IRoleService roleService, string roleId, [FromBody] IEnumerable<string> userIds) =>
            {
                if (userIds == null || !userIds.Any())
                {
                    return Results.BadRequest(BaseResponse<bool>.Failure("User IDs are required.", 400));
                }

                try
                {
                    var success = await roleService.RemoveUsersFromRoleAsync(roleId, userIds);
                    if (!success)
                    {
                        return Results.NotFound(BaseResponse<bool>.Failure("Failed to remove users or no such role found.", 404));
                    }
                    return Results.NotFound(BaseResponse<bool>.Success( true, 200));
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(BaseResponse<bool>.Failure(ex.Message, 500));
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get Role By UserId
            // Endpoint to get the role assigned to a specific user by user ID
            roleGroup.MapGet("/user/{userId}", async (IRoleService roleService, string userId) =>
            {
                try
                {
                    // Fetch role by user ID
                    var role = await roleService.GetRoleByUserIdAsync(userId);

                    if (role == null)
                    {
                        return Results.NotFound(BaseResponse<IEnumerable<string>>.Failure($"No role found for user with ID {userId}.", 404));
                    }

                    return Results.Ok(BaseResponse<IEnumerable<string>>.Success(role));
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(BaseResponse<IEnumerable<string>>.Failure(ex.Message, 500));
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
