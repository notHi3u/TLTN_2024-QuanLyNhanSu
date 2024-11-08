using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using EMS.Domain.Filters.EMS;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            attendanceGroup.MapGet("/{id}", async (IAttendanceService attendanceService, long id) =>
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
            attendanceGroup.MapPut("/{id}", async (IAttendanceService attendanceService, long id, [FromBody] AttendanceRequestDto updateAttendanceDto) =>
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
            attendanceGroup.MapDelete("/{id}", async (IAttendanceService attendanceService, long id) =>
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

            #region Get by EmployeeId
            attendanceGroup.MapGet("/employee/{employeeId}", async (IAttendanceService attendanceService, string employeeId) =>
            {
                try
                {
                    var attendances = await attendanceService.GetAttendancesByEmployIdAsync(employeeId);
                    if (attendances == null || !attendances.Any())
                    {
                        var errorResponse = BaseResponse<IEnumerable<AttendanceResponseDto>>.Failure("No attendances found for this employee.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<IEnumerable<AttendanceResponseDto>>.Success(attendances));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<IEnumerable<AttendanceResponseDto>>.Failure("An error occurred while retrieving the attendances for the employee.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion


            #region Get by TimeCardId
            attendanceGroup.MapGet("/timecard/{timeCardId:long}", async (IAttendanceService attendanceService, long timeCardId) =>
            {
                try
                {
                    var attendances = await attendanceService.GetAttendancesByTimeCardIdAsync(timeCardId);
                    if (attendances == null || !attendances.Any())
                    {
                        var errorResponse = BaseResponse<IEnumerable<AttendanceResponseDto>>.Failure("No attendances found for this timecard.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<IEnumerable<AttendanceResponseDto>>.Success(attendances));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<IEnumerable<AttendanceResponseDto>>.Failure("An error occurred while retrieving the attendances for the timecard.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete Bulk Endpoint
            attendanceGroup.MapDelete("/bulk", async (IAttendanceService attendanceService, [FromBody] List<long> ids) =>
            {
                if (ids == null || !ids.Any())
                {
                    var errorResponse = BaseResponse<bool>.Failure("No attendance IDs provided.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    // Call the DeleteBulkAsync method
                    var deletedCount = await attendanceService.DeleteBulkAsync(ids);

                    if (deletedCount == 0)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("No attendances were deleted.");
                        return Results.NotFound(errorResponse);
                    }

                    return Results.Ok(BaseResponse<int>.Success(deletedCount));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure($"An error occurred while deleting attendances: {ex.Message}");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

        }
    }
}
