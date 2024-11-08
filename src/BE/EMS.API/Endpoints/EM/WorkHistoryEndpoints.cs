using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class WorkHistoryEndpoints
    {
        public static void Map(WebApplication app)
        {
            var workHistoryGroup = app.MapGroup("/workhistory")
                .WithTags("WorkHistory");

            //#region Get WorkHistory By Employee Id
            //workHistoryGroup.MapGet("/employee/{employeeId:guid}", async (IWorkHistoryService workHistoryService, string employeeId) =>
            //{
            //    try
            //    {
            //        var workHistories = await workHistoryService.GetWorkHistoryByEmployeeIdAsync(employeeId);
            //        if (workHistories == null || !workHistories.Any())
            //        {
            //            var errorResponse = BaseResponse<IEnumerable<WorkHistoryResponseDto>>.Failure("Work history not found for the specified employee.");
            //            return Results.NotFound(errorResponse);
            //        }
            //        return Results.Ok(BaseResponse<IEnumerable<WorkHistoryResponseDto>>.Success(workHistories));
            //    }
            //    catch (Exception ex)
            //    {
            //        var errorResponse = BaseResponse<IEnumerable<WorkHistoryResponseDto>>.Failure("An error occurred while retrieving work history.");
            //        return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
            //    }
            //}).ConfigureApiResponses();
            //#endregion

            #region Create WorkHistory
            workHistoryGroup.MapPost("/", async (IWorkHistoryService workHistoryService, [FromBody] WorkHistoryRequestDto createWorkHistoryDto) =>
            {
                if (createWorkHistoryDto == null)
                {
                    var errorResponse = BaseResponse<WorkHistoryResponseDto>.Failure("Work history data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var workHistory = await workHistoryService.CreateWorkHistoryAsync(createWorkHistoryDto);
                    return Results.Ok(BaseResponse<WorkHistoryResponseDto>.Success(workHistory));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<WorkHistoryResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<WorkHistoryResponseDto>.Failure("An error occurred while creating the work history.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Update WorkHistory
            workHistoryGroup.MapPut("/{id:guid}", async (IWorkHistoryService workHistoryService, string id, [FromBody] WorkHistoryRequestDto updateWorkHistoryDto) =>
            {
                if (updateWorkHistoryDto == null)
                {
                    var errorResponse = BaseResponse<WorkHistoryResponseDto>.Failure("Work history data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var updatedWorkHistory = await workHistoryService.UpdateWorkHistoryAsync(id, updateWorkHistoryDto);
                    if (updatedWorkHistory == null)
                    {
                        var errorResponse = BaseResponse<WorkHistoryResponseDto>.Failure("Work history not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<WorkHistoryResponseDto>.Success(updatedWorkHistory));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<WorkHistoryResponseDto>.Failure("An error occurred while updating the work history.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete WorkHistory
            workHistoryGroup.MapDelete("/{id:guid}", async (IWorkHistoryService workHistoryService, string id) =>
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
