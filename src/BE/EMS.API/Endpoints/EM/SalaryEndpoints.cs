using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using EMS.Domain.Filters.EMS;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class SalaryEndpoints
    {
        public static void Map(WebApplication app)
        {
            var salaryGroup = app.MapGroup("/salaries")
                .WithTags("Salary");

            #region Get All Salaries
            salaryGroup.MapGet("/", async (ISalaryService salaryService, [AsParameters] SalaryFilter filter) =>
            {
                try
                {
                    filter ??= new SalaryFilter();
                    var pagedSalaries = await salaryService.GetPagedSalariesAsync(filter);
                    var response = BaseResponse<PagedDto<SalaryResponseDto>>.Success(pagedSalaries);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<PagedDto<SalaryResponseDto>>.Failure("An error occurred while retrieving salaries.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get Salary By Id
            salaryGroup.MapGet("/{id:guid}", async (ISalaryService salaryService, string id) =>
            {
                try
                {
                    var salary = await salaryService.GetSalaryByIdAsync(id);
                    if (salary == null)
                    {
                        var errorResponse = BaseResponse<SalaryResponseDto>.Failure("Salary not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<SalaryResponseDto>.Success(salary));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<SalaryResponseDto>.Failure("An error occurred while retrieving the salary.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Create Salary
            salaryGroup.MapPost("/", async (ISalaryService salaryService, [FromBody] SalaryRequestDto createSalaryDto) =>
            {
                if (createSalaryDto == null)
                {
                    var errorResponse = BaseResponse<SalaryResponseDto>.Failure("Salary data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var salary = await salaryService.CreateSalaryAsync(createSalaryDto);
                    return Results.Ok(BaseResponse<SalaryResponseDto>.Success(salary));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<SalaryResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<SalaryResponseDto>.Failure("An error occurred while creating the salary.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Update Salary
            salaryGroup.MapPut("/{id:guid}", async (ISalaryService salaryService, string id, [FromBody] SalaryRequestDto updateSalaryDto) =>
            {
                if (updateSalaryDto == null)
                {
                    var errorResponse = BaseResponse<SalaryResponseDto>.Failure("Salary data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var updatedSalary = await salaryService.UpdateSalaryAsync(id, updateSalaryDto);
                    return Results.Ok(BaseResponse<SalaryResponseDto>.Success(updatedSalary));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<SalaryResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<SalaryResponseDto>.Failure("An error occurred while updating the salary.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete Salary
            salaryGroup.MapDelete("/{id:guid}", async (ISalaryService salaryService, string id) =>
            {
                try
                {
                    var isDeleted = await salaryService.DeleteSalaryAsync(id);
                    if (!isDeleted)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Salary not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while deleting the salary.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
