using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using EMS.Domain.Filters.EMS;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class WorkRecordEndpoints
    {
        public static void Map(WebApplication app)
        {
            var workRecordGroup = app.MapGroup("/workrecord") // Changed "workHistoryGroup" to "workRecordGroup"
                .WithTags("WorkRecords");

            #region Get by Id
            workRecordGroup.MapGet("/{id}", async (IWorkRecordService workRecordService, long id) =>
            {
                try
                {
                    var workRecord = await workRecordService.GetWorkRecordByIdAsync(id); // Changed method name to "GetWorkRecordByIdAsync"
                    if (workRecord == null)
                    {
                        var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("Work record not found."); // Updated error message
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<WorkRecordResponseDto>.Success(workRecord));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("An error occurred while retrieving work record."); // Updated error message
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get All
            workRecordGroup.MapGet("/", async (IWorkRecordService workRecordService, [AsParameters] WorkRecordFilter filter) =>
            {
                try
                {
                    filter ??= new WorkRecordFilter();
                    var pagedWorkRecords = await workRecordService.GetPagedWorkRecordsAsync(filter); // Changed method name to "GetPagedWorkRecordsAsync"
                    var response = BaseResponse<PagedDto<WorkRecordResponseDto>>.Success(pagedWorkRecords);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<PagedDto<WorkRecordResponseDto>>.Failure("An error occurred while retrieving work records."); // Updated error message
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Create WorkRecord
            workRecordGroup.MapPost("/", async (IWorkRecordService workRecordService, [FromBody] WorkRecordRequestDto createWorkRecordDto) =>
            {
                if (createWorkRecordDto == null)
                {
                    var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("Work record data is required."); // Updated error message
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var workRecord = await workRecordService.CreateWorkRecordAsync(createWorkRecordDto); // Changed method name to "CreateWorkRecordAsync"
                    return Results.Ok(BaseResponse<WorkRecordResponseDto>.Success(workRecord));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("An error occurred while creating the work record."); // Updated error message
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Update WorkRecord
            workRecordGroup.MapPut("/{id}", async (IWorkRecordService workRecordService, long id, [FromBody] WorkRecordRequestDto updateWorkRecordDto) =>
            {
                if (updateWorkRecordDto == null)
                {
                    var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("Work record data is required."); // Updated error message
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var updatedWorkRecord = await workRecordService.UpdateWorkRecordAsync(id, updateWorkRecordDto); // Changed method name to "UpdateWorkRecordAsync"
                    if (updatedWorkRecord == null)
                    {
                        var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("Work record not found."); // Updated error message
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<WorkRecordResponseDto>.Success(updatedWorkRecord));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("An error occurred while updating the work record."); // Updated error message
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete WorkRecord
            workRecordGroup.MapDelete("/{id}", async (IWorkRecordService workRecordService, long id) =>
            {
                try
                {
                    var isDeleted = await workRecordService.DeleteWorkRecordAsync(id); // Changed method name to "DeleteWorkRecordAsync"
                    if (!isDeleted)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Work record not found."); // Updated error message
                        return Results.NotFound(errorResponse);
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while deleting the work record."); // Updated error message
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
