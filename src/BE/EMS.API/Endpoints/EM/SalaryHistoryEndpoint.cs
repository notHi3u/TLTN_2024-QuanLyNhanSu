using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class SalaryHistoryEndpoints
    {
        public static void Map(WebApplication app)
        {
            var salaryHistoryGroup = app.MapGroup("/salary-record")
                .WithTags("SalaryRecord");

            //#region Get Salary History By Employee Id
            //salaryHistoryGroup.MapGet("/employee/{employeeId:guid}", async (ISalaryHistoryService salaryHistoryService, string employeeId, DateTime? startDate, DateTime? endDate) =>
            //{
            //    try
            //    {
            //        var salaryHistory = await salaryHistoryService.GetSalaryHistoryByIdAsync(employeeId);
            //        if (salaryHistory == null || !salaryHistory.Any())
            //        {
            //            var errorResponse = BaseResponse<IEnumerable<SalaryHistoryResponseDto>>.Failure("Salary history not found.");
            //            return Results.NotFound(errorResponse);
            //        }
            //        return Results.Ok(BaseResponse<IEnumerable<SalaryHistoryResponseDto>>.Success(salaryHistory));
            //    }
            //    catch (Exception ex)
            //    {
            //        var errorResponse = BaseResponse<IEnumerable<SalaryHistoryResponseDto>>.Failure("An error occurred while retrieving salary history.");
            //        return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
            //    }
            //}).ConfigureApiResponses();
            //#endregion

            #region Add New Salary Record
            salaryHistoryGroup.MapPost("/", async (ISalaryRecordService salaryHistoryService, [FromBody] SalaryRecordRequestDto newSalaryDto) =>
            {
                if (newSalaryDto == null)
                {
                    var errorResponse = BaseResponse<SalaryRecordResponseDto>.Failure("Salary data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var salaryHistoryEntry = await salaryHistoryService.CreateSalaryHistoryAsync(newSalaryDto);
                    return Results.Ok(BaseResponse<SalaryRecordResponseDto>.Success(salaryHistoryEntry));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<SalaryRecordResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<SalaryRecordResponseDto>.Failure("An error occurred while adding the salary record.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
