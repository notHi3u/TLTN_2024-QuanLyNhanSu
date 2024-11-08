using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using EMS.Domain.Filters.EMS;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class AttendanceEndpoints
    {
        public static void Map(WebApplication app)
        {
            var attendanceGroup = app.MapGroup("/attendances")
                .WithTags("Attendance");

            #region Get All Attendances
            attendanceGroup.MapGet("/", async (IAttendanceService attendanceService, [AsParameters] AttendanceFilter filter) =>
            {
                try
                {
                    filter ??= new AttendanceFilter();
                    var pagedAttendances = await attendanceService.GetPagedAttendancesAsync(filter);
                    var response = BaseResponse<PagedDto<AttendanceResponseDto>>.Success(pagedAttendances);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<PagedDto<AttendanceResponseDto>>.Failure("An error occurred while retrieving attendances.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get Attendance By Id
            attendanceGroup.MapGet("/{id:guid}", async (IAttendanceService attendanceService, string id) =>
            {
                try
                {
                    var attendance = await attendanceService.GetAttendanceByIdAsync(id);
                    if (attendance == null)
                    {
                        var errorResponse = BaseResponse<AttendanceResponseDto>.Failure("Attendance not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<AttendanceResponseDto>.Success(attendance));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<AttendanceResponseDto>.Failure("An error occurred while retrieving the attendance.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Create Attendance
            attendanceGroup.MapPost("/", async (IAttendanceService attendanceService, [FromBody] AttendanceRequestDto createAttendanceDto) =>
            {
                if (createAttendanceDto == null)
                {
                    var errorResponse = BaseResponse<AttendanceResponseDto>.Failure("Attendance data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var attendance = await attendanceService.CreateAttendanceAsync(createAttendanceDto);
                    return Results.Ok(BaseResponse<AttendanceResponseDto>.Success(attendance));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<AttendanceResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<AttendanceResponseDto>.Failure("An error occurred while creating the attendance.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Update Attendance
            attendanceGroup.MapPut("/{id:guid}", async (IAttendanceService attendanceService, string id, [FromBody] AttendanceRequestDto updateAttendanceDto) =>
            {
                if (updateAttendanceDto == null)
                {
                    var errorResponse = BaseResponse<AttendanceResponseDto>.Failure("Attendance data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var updatedAttendance = await attendanceService.UpdateAttendanceAsync(id, updateAttendanceDto);
                    return Results.Ok(BaseResponse<AttendanceResponseDto>.Success(updatedAttendance));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<AttendanceResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<AttendanceResponseDto>.Failure("An error occurred while updating the attendance.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete Attendance
            attendanceGroup.MapDelete("/{id:guid}", async (IAttendanceService attendanceService, string id) =>
            {
                try
                {
                    var isDeleted = await attendanceService.DeleteAttendanceAsync(id);
                    if (!isDeleted)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Attendance not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while deleting the attendance.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
