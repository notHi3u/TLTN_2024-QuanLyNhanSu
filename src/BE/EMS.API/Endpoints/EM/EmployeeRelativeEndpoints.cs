using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using EMS.Domain.Filters.EMS;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class EmployeeRelativeEndpoints
    {
        public static void Map(WebApplication app)
        {
            var relativeGroup = app.MapGroup("/employee-relatives")
                .WithTags("Employee Relative");

            #region Get All Employee Relatives
            relativeGroup.MapGet("/", async (IEmployeeRelativeService relativeService, [AsParameters] EmployeeRelativeFilter filter) =>
            {
                try
                {
                    filter ??= new EmployeeRelativeFilter();
                    var pagedRelatives = await relativeService.GetPagedEmployeeRelativesAsync(filter);
                    var response = BaseResponse<PagedDto<EmployeeRelativeResponseDto>>.Success(pagedRelatives);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<PagedDto<EmployeeRelativeResponseDto>>.Failure("An error occurred while retrieving employee relatives.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            //#region Get Employee Relative By Id
            //relativeGroup.MapGet("/{id:guid}", async (IEmployeeRelativeService relativeService, string id) =>
            //{
            //    try
            //    {
            //        var relative = await relativeService.GetEmployeeRelativeByIdAsync(id);
            //        if (relative == null)
            //        {
            //            var errorResponse = BaseResponse<EmployeeRelativeResponseDto>.Failure("Employee relative not found.");
            //            return Results.NotFound(errorResponse);
            //        }
            //        return Results.Ok(BaseResponse<EmployeeRelativeResponseDto>.Success(relative));
            //    }
            //    catch (Exception ex)
            //    {
            //        var errorResponse = BaseResponse<EmployeeRelativeResponseDto>.Failure("An error occurred while retrieving the employee relative.");
            //        return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
            //    }
            //}).ConfigureApiResponses();
            //#endregion

            #region Create Employee Relative
            relativeGroup.MapPost("/", async (IEmployeeRelativeService relativeService, [FromBody] EmployeeRelativeRequestDto createRelativeDto) =>
            {
                if (createRelativeDto == null)
                {
                    var errorResponse = BaseResponse<EmployeeRelativeResponseDto>.Failure("Employee relative data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var relative = await relativeService.CreateEmployeeRelativeAsync(createRelativeDto);
                    return Results.Ok(BaseResponse<EmployeeRelativeResponseDto>.Success(relative));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<EmployeeRelativeResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<EmployeeRelativeResponseDto>.Failure("An error occurred while creating the employee relative.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Update Employee Relative
            relativeGroup.MapPut("/{id:guid}", async (IEmployeeRelativeService relativeService, string id, [FromBody] EmployeeRelativeRequestDto updateRelativeDto) =>
            {
                if (updateRelativeDto == null)
                {
                    var errorResponse = BaseResponse<EmployeeRelativeResponseDto>.Failure("Employee relative data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var updatedRelative = await relativeService.UpdateEmployeeRelativeAsync(id, updateRelativeDto);
                    return Results.Ok(BaseResponse<EmployeeRelativeResponseDto>.Success(updatedRelative));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<EmployeeRelativeResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<EmployeeRelativeResponseDto>.Failure("An error occurred while updating the employee relative.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete Employee Relative
            relativeGroup.MapDelete("/{id:guid}", async (IEmployeeRelativeService relativeService, string id) =>
            {
                try
                {
                    var isDeleted = await relativeService.DeleteEmployeeRelativeAsync(id);
                    if (!isDeleted)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Employee relative not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while deleting the employee relative.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
