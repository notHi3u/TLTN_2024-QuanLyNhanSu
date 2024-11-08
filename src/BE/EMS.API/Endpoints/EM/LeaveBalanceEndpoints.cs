using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using EMS.Domain.Filters.EMS;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class LeaveBalanceEndpoints
    {
        public static void Map(WebApplication app)
        {
            var leaveBalanceGroup = app.MapGroup("/leave-balances")
                .WithTags("Leave Balance");

            #region Get All Leave Balances
            leaveBalanceGroup.MapGet("/", async (ILeaveBalanceService leaveBalanceService, [AsParameters] LeaveBalanceFilter filter) =>
            {
                try
                {
                    filter ??= new LeaveBalanceFilter();
                    var pagedBalances = await leaveBalanceService.GetPagedLeaveBalancesAsync(filter);
                    var response = BaseResponse<PagedDto<LeaveBalanceResponseDto>>.Success(pagedBalances);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<PagedDto<LeaveBalanceResponseDto>>.Failure("An error occurred while retrieving leave balances.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get Leave Balance By Employee Id
            leaveBalanceGroup.MapGet("/{employeeId:guid}", async (ILeaveBalanceService leaveBalanceService, string employeeId) =>
            {
                try
                {
                    var leaveBalance = await leaveBalanceService.GetLeaveBalanceByIdAsync(employeeId);
                    if (leaveBalance == null)
                    {
                        var errorResponse = BaseResponse<LeaveBalanceResponseDto>.Failure("Leave balance not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<LeaveBalanceResponseDto>.Success(leaveBalance));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<LeaveBalanceResponseDto>.Failure("An error occurred while retrieving the leave balance.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Create Leave Balance
            leaveBalanceGroup.MapPost("/", async (ILeaveBalanceService leaveBalanceService, [FromBody] LeaveBalanceRequestDto createLeaveBalanceDto) =>
            {
                if (createLeaveBalanceDto == null)
                {
                    var errorResponse = BaseResponse<LeaveBalanceResponseDto>.Failure("Leave balance data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var leaveBalance = await leaveBalanceService.CreateLeaveBalanceAsync(createLeaveBalanceDto);
                    return Results.Ok(BaseResponse<LeaveBalanceResponseDto>.Success(leaveBalance));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<LeaveBalanceResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<LeaveBalanceResponseDto>.Failure("An error occurred while creating the leave balance.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Update Leave Balance
            leaveBalanceGroup.MapPut("/{employeeId:guid}", async (ILeaveBalanceService leaveBalanceService, string employeeId, [FromBody] LeaveBalanceRequestDto updateLeaveBalanceDto) =>
            {
                if (updateLeaveBalanceDto == null)
                {
                    var errorResponse = BaseResponse<LeaveBalanceResponseDto>.Failure("Leave balance data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var updatedBalance = await leaveBalanceService.UpdateLeaveBalanceAsync(employeeId, updateLeaveBalanceDto);
                    return Results.Ok(BaseResponse<LeaveBalanceResponseDto>.Success(updatedBalance));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<LeaveBalanceResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<LeaveBalanceResponseDto>.Failure("An error occurred while updating the leave balance.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete Leave Balance
            leaveBalanceGroup.MapDelete("/{employeeId:guid}", async (ILeaveBalanceService leaveBalanceService, string employeeId) =>
            {
                try
                {
                    var isDeleted = await leaveBalanceService.DeleteLeaveBalanceAsync(employeeId);
                    if (!isDeleted)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Leave balance not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while deleting the leave balance.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
