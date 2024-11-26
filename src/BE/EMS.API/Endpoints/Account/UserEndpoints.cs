using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.Account;
using EMS.Application.Services.Account;
using EMS.Domain.Filters.Account;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.Account
{
    public static class UserEndpoints
    {
        public static void Map(WebApplication app)
        {
            var userGroup = app.MapGroup("/users")
                .WithTags("User");

            #region Get All Users
            userGroup.MapGet("/", async ([FromServices] IUserService userService, [AsParameters] UserFilter filter) =>
            {
                try
                {
                    filter ??= new UserFilter();
                    var pagedUsers = await userService.GetPagedUsersAsync(filter);
                    var response = BaseResponse<PagedDto<UserResponseDto>>.Success(pagedUsers);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    var errorResponse = BaseResponse<PagedDto<UserResponseDto>>.Failure("An error occurred while retrieving users.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            })
                .RequireAuthorization("Admin", "HR")
                .ConfigureApiResponses();
            #endregion

            #region Get User By Id
            userGroup.MapGet("/{id}", async (IUserService userService, string id, bool? isDeep) =>
            {
                try
                {
                    var userResponse = await userService.GetUserByIdAsync(id, isDeep);
                    if (!userResponse.IsSuccess)
                    {
                        var errorResponse = BaseResponse<UserResponseDto>.Failure("User not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(userResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<UserResponseDto>.Failure("An error occurred while retrieving the user.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            })
                .RequireAuthorization()
                .ConfigureApiResponses();
            #endregion

            #region Create User
            userGroup.MapPost("/", async (IUserService userService, [FromBody] UserRequestDto createUserDto) =>
            {
                if (createUserDto == null)
                {
                    var errorResponse = BaseResponse<UserResponseDto>.Failure("User data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var userResponse = await userService.CreateUserAsync(createUserDto);
                    if (!userResponse.IsSuccess)
                    {
                        var errorResponse = BaseResponse<UserResponseDto>.Failure("User creation failed.");
                        return Results.BadRequest(errorResponse);
                    }
                    return Results.Ok(userResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<UserResponseDto>.Failure("An error occurred while creating the user.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            })
                .RequireAuthorization("Admin", "HR")
                .ConfigureApiResponses();
            #endregion

            #region Update User
            userGroup.MapPut("/{id}", async (IUserService userService, string id, [FromBody] UserRequestDto updateUserDto) =>
            {
                if (updateUserDto == null)
                {
                    var errorResponse = BaseResponse<UserResponseDto>.Failure("User data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var userResponse = await userService.UpdateUserAsync(id, updateUserDto);
                    if (!userResponse.IsSuccess)
                    {
                        var errorResponse = BaseResponse<UserResponseDto>.Failure("User not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(userResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<UserResponseDto>.Failure("An error occurred while updating the user.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            })
                .RequireAuthorization("Admin", "HR")
                .ConfigureApiResponses();
            #endregion

            #region Delete User
            userGroup.MapDelete("/{id}", async (IUserService userService, string id) =>
            {
                try
                {
                    var response = await userService.DeleteUserAsync(id);
                    if (!response.IsSuccess)
                    {
                        var errorResponse = BaseResponse<UserResponseDto>.Failure("User not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<UserResponseDto>.Failure("An error occurred while deleting the user.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            })
                .RequireAuthorization("Admin", "HR")
                .ConfigureApiResponses();
            #endregion

            #region Assign Single Role to User
            userGroup.MapPost("/{userid}/role", async (IUserService userService, string userId, string roleName) =>
            {
                try
                {
                    var response = await userService.AssignRoleToUserAsync(userId, roleName);
                    if (!response.IsSuccess)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Failed to assign role.");
                        return Results.BadRequest(errorResponse);
                    }
                    return Results.Ok(BaseResponse<bool>.Success(true, 200));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while assigning the role.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            })
                .RequireAuthorization("Admin", "HR")
                .ConfigureApiResponses();
            #endregion

            #region Assign Multiple Roles to User
            userGroup.MapPost("/{userid}/roles", async (IUserService userService, string userId, [FromBody] IEnumerable<string> roleNames) =>
            {
                try
                {
                    var response = await userService.AssignRolesToUserAsync(userId, roleNames);
                    if (!response.IsSuccess)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Failed to assign roles.");
                        return Results.BadRequest(errorResponse);
                    }
                    return Results.Ok(BaseResponse<bool>.Success(true, 200));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while assigning the roles.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            })
                .RequireAuthorization("Admin", "HR")
                .ConfigureApiResponses();
            #endregion

            #region Remove Role from User
            userGroup.MapDelete("/{userid}/role", async (IUserService userService, string userId, string roleName) =>
            {
                try
                {
                    var response = await userService.RemoveRoleFromUserAsync(userId, roleName);
                    if (!response.IsSuccess)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Failed to remove role.");
                        return Results.BadRequest(errorResponse);
                    }
                    return Results.Ok(BaseResponse<bool>.Success(true, 200));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while removing the role.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            })
                .RequireAuthorization("Admin", "HR")
                .ConfigureApiResponses();
            #endregion

            #region Remove Multiple Roles from User
            userGroup.MapDelete("/{userid}/roles", async (IUserService userService, string userId, [FromBody] IEnumerable<string> roleNames) =>
            {
                try
                {
                    var response = await userService.RemoveRolesFromUserAsync(userId, roleNames);
                    if (!response.IsSuccess)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Failed to remove roles.");
                        return Results.BadRequest(errorResponse);
                    }
                    return Results.Ok(BaseResponse<bool>.Success(true, 200));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while removing roles.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            })
                .RequireAuthorization("Admin", "HR")
                .ConfigureApiResponses();
            #endregion
        }
    }
}
