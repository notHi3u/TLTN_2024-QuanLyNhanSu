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
            departmentGroup.MapGet("/{id}", async (IDepartmentService departmentService, string id, bool? isDeep) =>
            {
                try
                {
                    var department = await departmentService.GetDepartmentByIdAsync(id, isDeep);
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
            departmentGroup.MapPut("/{id}", async (IDepartmentService departmentService, string id, [FromBody] DepartmentRequestDto updateDepartmentDto) =>
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
            departmentGroup.MapDelete("/{id}", async (IDepartmentService departmentService, string id) =>
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

            #region Assign Manager to Department
            departmentGroup.MapPost("/{id}/assign-manager", async (IDepartmentService departmentService, string id, [FromBody] AssignManagerRequestDto assignManagerDto) =>
            {
                if (assignManagerDto == null || string.IsNullOrWhiteSpace(assignManagerDto.ManagerId))
                {
                    var errorResponse = BaseResponse<DepartmentResponseDto>.Failure("Manager data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var department = await departmentService.AssignManagerAsync(id, assignManagerDto.ManagerId);
                    if (department == null)
                    {
                        var errorResponse = BaseResponse<DepartmentResponseDto>.Failure("Department not found.");
                        return Results.NotFound(errorResponse);
                    }

                    return Results.Ok(BaseResponse<DepartmentResponseDto>.Success(department));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<DepartmentResponseDto>.Failure("An error occurred while assigning the manager.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Remove Manager from Department
            departmentGroup.MapPost("/{id}/remove-manager", async (IDepartmentService departmentService, string id) =>
            {
                try
                {
                    var department = await departmentService.RemoveManagerAsync(id);
                    if (department == null)
                    {
                        var errorResponse = BaseResponse<DepartmentResponseDto>.Failure("Department not found.");
                        return Results.NotFound(errorResponse);
                    }

                    return Results.Ok(BaseResponse<DepartmentResponseDto>.Success(department));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<DepartmentResponseDto>.Failure("An error occurred while removing the manager.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get Employees by Department
            departmentGroup.MapGet("/{id}/employees", async (IDepartmentService departmentService, string id) =>
            {
                try
                {
                    var employees = await departmentService.GetEmployeesByDepartmentAsync(id);
                    if (employees == null || !employees.Any())
                    {
                        var errorResponse = BaseResponse<IEnumerable<EmployeeResponseDto>>.Failure("No employees found in this department.");
                        return Results.NotFound(errorResponse);
                    }

                    return Results.Ok(BaseResponse<IEnumerable<EmployeeResponseDto>>.Success(employees));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<IEnumerable<EmployeeResponseDto>>.Failure("An error occurred while retrieving the employees.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get Department Manager
            departmentGroup.MapGet("/{id}/manager", async (IDepartmentService departmentService, string id) =>
            {
                try
                {
                    var manager = await departmentService.GetDepartmentManagerAsync(id);
                    if (manager == null)
                    {
                        var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("Department manager not found.");
                        return Results.NotFound(errorResponse);
                    }

                    return Results.Ok(BaseResponse<EmployeeResponseDto>.Success(manager));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("An error occurred while retrieving the department manager.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
