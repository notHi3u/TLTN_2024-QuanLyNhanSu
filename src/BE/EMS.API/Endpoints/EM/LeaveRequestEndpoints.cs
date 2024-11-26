using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using EMS.Domain.Filters.EMS;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class LeaveRequestEndpoints
    {
        public static void Map(WebApplication app)
        {
            var leaveRequestGroup = app.MapGroup("/leave-requests")
                .WithTags("Leave Request")
                .RequireAuthorization();

            #region Get All Leave Requests
            leaveRequestGroup.MapGet("/", async (ILeaveRequestService leaveRequestService, [AsParameters] LeaveRequestFilter filter) =>
            {
                try
                {
                    filter ??= new LeaveRequestFilter();
                    var pagedLeaveRequests = await leaveRequestService.GetPagedLeaveRequestsAsync(filter);
                    var response = BaseResponse<PagedDto<LeaveRequestResponseDto>>.Success(pagedLeaveRequests);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<PagedDto<LeaveRequestResponseDto>>.Failure("An error occurred while retrieving leave requests.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get Leave Request By Id
            leaveRequestGroup.MapGet("/{id}", async (ILeaveRequestService leaveRequestService, long id) =>
            {
                try
                {
                    var leaveRequest = await leaveRequestService.GetLeaveRequestByIdAsync(id);
                    if (leaveRequest == null)
                    {
                        var errorResponse = BaseResponse<LeaveRequestResponseDto>.Failure("Leave request not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<LeaveRequestResponseDto>.Success(leaveRequest));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<LeaveRequestResponseDto>.Failure("An error occurred while retrieving the leave request.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Create Leave Request
            leaveRequestGroup.MapPost("/", async (ILeaveRequestService leaveRequestService, [FromBody] LeaveRequestRequestDto createLeaveRequestDto) =>
            {
                if (createLeaveRequestDto == null)
                {
                    var errorResponse = BaseResponse<LeaveRequestResponseDto>.Failure("Leave request data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var leaveRequest = await leaveRequestService.CreateLeaveRequestAsync(createLeaveRequestDto);
                    return Results.Ok(BaseResponse<LeaveRequestResponseDto>.Success(leaveRequest));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<LeaveRequestResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<LeaveRequestResponseDto>.Failure("An error occurred while creating the leave request.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Update Leave Request
            leaveRequestGroup.MapPut("/{id}", async (ILeaveRequestService leaveRequestService, long id, [FromBody] LeaveRequestRequestDto updateLeaveRequestDto) =>
            {
                if (updateLeaveRequestDto == null)
                {
                    var errorResponse = BaseResponse<LeaveRequestResponseDto>.Failure("Leave request data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var updatedLeaveRequest = await leaveRequestService.UpdateLeaveRequestAsync(id, updateLeaveRequestDto);
                    return Results.Ok(BaseResponse<LeaveRequestResponseDto>.Success(updatedLeaveRequest));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<LeaveRequestResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<LeaveRequestResponseDto>.Failure("An error occurred while updating the leave request.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete Leave Request
            leaveRequestGroup.MapDelete("/{id}", async (ILeaveRequestService leaveRequestService, long id) =>
            {
                try
                {
                    var isDeleted = await leaveRequestService.DeleteLeaveRequestAsync(id);
                    if (!isDeleted)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Leave request not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while deleting the leave request.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
