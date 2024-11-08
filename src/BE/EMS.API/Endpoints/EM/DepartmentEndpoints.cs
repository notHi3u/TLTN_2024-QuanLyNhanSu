using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using EMS.Domain.Filters.EMS;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class DepartmentEndpoints
    {
        public static void Map(WebApplication app)
        {
            var departmentGroup = app.MapGroup("/departments")
                .WithTags("Department");

            #region Get All Departments
            departmentGroup.MapGet("/", async (IDepartmentService departmentService, [AsParameters] DepartmentFilter filter) =>
            {
                try
                {
                    filter ??= new DepartmentFilter();
                    var pagedDepartments = await departmentService.GetPagedDepartmentsAsync(filter);
                    var response = BaseResponse<PagedDto<DepartmentResponseDto>>.Success(pagedDepartments);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<PagedDto<DepartmentResponseDto>>.Failure("An error occurred while retrieving departments.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get Department By Id
            departmentGroup.MapGet("/{id:guid}", async (IDepartmentService departmentService, string id) =>
            {
                try
                {
                    var department = await departmentService.GetDepartmentByIdAsync(id);
                    if (department == null)
                    {
                        var errorResponse = BaseResponse<DepartmentResponseDto>.Failure("Department not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<DepartmentResponseDto>.Success(department));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<DepartmentResponseDto>.Failure("An error occurred while retrieving the department.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Create Department
            departmentGroup.MapPost("/", async (IDepartmentService departmentService, [FromBody] DepartmentRequestDto createDepartmentDto) =>
            {
                if (createDepartmentDto == null)
                {
                    var errorResponse = BaseResponse<DepartmentResponseDto>.Failure("Department data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var department = await departmentService.CreateDepartmentAsync(createDepartmentDto);
                    return Results.Ok(BaseResponse<DepartmentResponseDto>.Success(department));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<DepartmentResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<DepartmentResponseDto>.Failure("An error occurred while creating the department.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Update Department
            departmentGroup.MapPut("/{id:guid}", async (IDepartmentService departmentService, string id, [FromBody] DepartmentRequestDto updateDepartmentDto) =>
            {
                if (updateDepartmentDto == null)
                {
                    var errorResponse = BaseResponse<DepartmentResponseDto>.Failure("Department data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var updatedDepartment = await departmentService.UpdateDepartmentAsync(id, updateDepartmentDto);
                    return Results.Ok(BaseResponse<DepartmentResponseDto>.Success(updatedDepartment));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<DepartmentResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<DepartmentResponseDto>.Failure("An error occurred while updating the department.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete Department
            departmentGroup.MapDelete("/{id:guid}", async (IDepartmentService departmentService, string id) =>
            {
                try
                {
                    var isDeleted = await departmentService.DeleteDepartmentAsync(id);
                    if (!isDeleted)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Department not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while deleting the department.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
