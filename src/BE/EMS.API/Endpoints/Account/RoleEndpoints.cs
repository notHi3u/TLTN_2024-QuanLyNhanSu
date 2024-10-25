using EMS.Application.DTOs.Account;
using EMS.Application.Services.Account;
using EMS.Domain.Filters.Account;
using EMS.Domain.Models.Account;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.Account
{
    public static class RoleEndpoints
    {
        public static void Map(WebApplication app)
        {
            #region Define
            var userGroup = app.MapGroup("/roles").WithTags("Role");
            //.RequireAuthorization();
            #endregion

            #region Get all roles
            // Endpoint để lấy danh sách các vai trò phân trang
            userGroup.MapGet("/", async ([FromServices] IRoleService roleService, [AsParameters] RoleFilter filter) =>
            {
                try
                {
                    var pagedRoles = await roleService.GetPagedRolesAsync(filter);
                    return Results.Ok(pagedRoles);
                }
                catch (Exception ex)
                {
                    // Log the exception if necessary
                    return Results.Problem(ex.Message);
                }
            });
            #endregion

            #region Get role by Id
            // Endpoint để lấy thông tin vai trò theo ID
            userGroup.MapGet("/{id:guid}", async (IRoleService roleService, string id, bool? IsDeep = false) =>
            {
                var role = await roleService.GetRoleByIdAsync(id, (bool)IsDeep);
                if (role == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(role);
            });
            #endregion

            #region Create Role
            // Endpoint để tạo một vai trò mới
            userGroup.MapPost("/", async (IRoleService roleService, [FromBody] RoleRequestDto createRoleDto) =>
            {
                if (createRoleDto == null)
                {
                    return Results.BadRequest("Role data is required.");
                }

                var role = await roleService.CreateRoleAsync(createRoleDto);
                return Results.Created($"/{role}", role);
            });
            #endregion

            #region Update role
            // Endpoint để cập nhật thông tin vai trò theo ID
            userGroup.MapPut("/{id:guid}", async (IRoleService roleService, string id, [FromBody] RoleRequestDto updateRoleDto) =>
            {
                if (updateRoleDto == null)
                {
                    return Results.BadRequest("Role data is required.");
                }

                var updatedRole = await roleService.UpdateRoleAsync(id, updateRoleDto);
                if (updatedRole == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(updatedRole);
            });
            #endregion

            #region Delete role
            // Endpoint để xóa vai trò theo ID
            userGroup.MapDelete("/{id:guid}", async (IRoleService roleService, string id) =>
            {
                var success = await roleService.DeleteRoleAsync(id);
                if (!success)
                {
                    return Results.NotFound();
                }
                return Results.NoContent();
            });
            #endregion

            #region Assign Permission
            userGroup.MapPost("/{roleId:guid}/permission", async (IRolePermissionService rolePermissionService, string roleId, string permissisonId) =>
            {
                if (string.IsNullOrEmpty(permissisonId))
                    return Results.BadRequest("Permission ID is required.");
                var newRolePermission = await rolePermissionService.AddRolePermissionAsync(roleId, permissisonId);
                if (newRolePermission == null)
                    return Results.NotFound();
                return Results.Ok(newRolePermission);
            });
            #endregion

            #region Revoke Permission
            userGroup.MapDelete("/{roleId:guid}/permission", async (IRolePermissionService rolePermissionService, string roleId, string permissisonId) =>
            {
                if (string.IsNullOrEmpty(permissisonId))
                    return Results.BadRequest("Permission ID is required.");
                var newRolePermission = await rolePermissionService.RemoveRolePermissionAsync(roleId, permissisonId);
                if (!newRolePermission)
                    return Results.NotFound();
                return Results.Ok();
            });
            #endregion

            #region Assign Role To Multiple Users
            userGroup.MapPost("/{roleId:guid}/users", async (IRoleService roleService, string roleId, [FromBody] IEnumerable<string> userIds) =>
            {
                try
                {
                    if (userIds == null || !userIds.Any())
                    {
                        return Results.BadRequest("At least one user ID must be provided.");
                    }

                    var response = await roleService.AssignRoleToMultipleUsersAsync(roleId, userIds);
                    if (!response.IsSuccess)
                    {
                        return Results.BadRequest(response.Errors);
                    }

                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    // Log the exception if necessary
                    return Results.Problem(ex.Message);
                }
            });
            #endregion

            #region Get Users By Role
            // Endpoint to retrieve all users assigned to a specific role
            userGroup.MapGet("/{roleId:guid}/users", async (IRoleService roleService, string roleId) =>
            {
                try
                {
                    var users = await roleService.GetUsersByRoleIdAsync(roleId);
                    if (users == null || !users.Any())
                    {
                        return Results.NotFound("No users found for this role.");
                    }
                    return Results.Ok(users);
                }
                catch (Exception ex)
                {
                    // Log the exception if necessary
                    return Results.Problem(ex.Message);
                }
            });
            #endregion

            #region Remove Multiple Users From Role
            // Endpoint to remove multiple users from a specific role
            userGroup.MapDelete("/{roleId:guid}/users", async (IRoleService roleService, string roleId, [FromBody] IEnumerable<string> userIds) =>
            {
                if (userIds == null || !userIds.Any())
                {
                    return Results.BadRequest("User IDs are required.");
                }

                try
                {
                    var success = await roleService.RemoveUsersFromRoleAsync(roleId, userIds);
                    if (!success)
                    {
                        return Results.NotFound("Failed to remove users from the role or no such role found.");
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    // Log the exception if necessary
                    return Results.Problem(ex.Message);
                }
            });
            #endregion

        }
    }
}
