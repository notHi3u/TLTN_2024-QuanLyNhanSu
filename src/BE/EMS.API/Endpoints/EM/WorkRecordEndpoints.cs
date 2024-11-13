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
            var workHistoryGroup = app.MapGroup("/workrecord")
                .WithTags("WorkRecords");

            #region Get by Id
            workHistoryGroup.MapGet("/{id}", async (IWorkRecordService workRecordService, long id) =>
            {
                try
                {
                    var workRecord = await workRecordService.GetWorkHistoryByIdAsync(id);
                    if (workRecord == null)
                    {
                        var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("Salary history not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<WorkRecordResponseDto>.Success(workRecord));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("An error occurred while retrieving Work record.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get All
            workHistoryGroup.MapGet("/", async (IWorkRecordService workRecordService, [AsParameters] WorkRecordFilter filter) =>
            {
                try
                {
                    filter ??= new WorkRecordFilter();
                    var pagedWorkRecords = await workRecordService.GetPagedWorkHistoriesAsync(filter);
                    var response = BaseResponse<PagedDto<WorkRecordResponseDto>>.Success(pagedWorkRecords);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<PagedDto<WorkRecordResponseDto>>.Failure("An error occurred while retrieving Work records.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Create WorkHistory
            workHistoryGroup.MapPost("/", async (IWorkRecordService workHistoryService, [FromBody] WorkRecordRequestDto createWorkHistoryDto) =>
            {
                if (createWorkHistoryDto == null)
                {
                    var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("Work history data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var workHistory = await workHistoryService.CreateWorkHistoryAsync(createWorkHistoryDto);
                    return Results.Ok(BaseResponse<WorkRecordResponseDto>.Success(workHistory));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("An error occurred while creating the work history.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Update WorkHistory
            workHistoryGroup.MapPut("/{id}", async (IWorkRecordService workHistoryService, long id, [FromBody] WorkRecordRequestDto updateWorkHistoryDto) =>
            {
                if (updateWorkHistoryDto == null)
                {
                    var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("Work history data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var updatedWorkHistory = await workHistoryService.UpdateWorkHistoryAsync(id, updateWorkHistoryDto);
                    if (updatedWorkHistory == null)
                    {
                        var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("Work history not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<WorkRecordResponseDto>.Success(updatedWorkHistory));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<WorkRecordResponseDto>.Failure("An error occurred while updating the work history.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete WorkHistory
            workHistoryGroup.MapDelete("/{id}", async (IWorkRecordService workHistoryService, long id) =>
            {
                try
                {
                    var isDeleted = await workHistoryService.DeleteWorkHistoryAsync(id);
                    if (!isDeleted)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Work history not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while deleting the work history.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
