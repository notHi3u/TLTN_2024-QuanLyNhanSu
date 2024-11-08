using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using EMS.Domain.Filters.EMS;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class EmployeeEndpoints
    {
        public static void Map(WebApplication app)
        {
            var employeeGroup = app.MapGroup("/employees")
                .WithTags("Employee");

            #region Get All Employees
            employeeGroup.MapGet("/", async (IEmployeeService employeeService, [AsParameters] EmployeeFilter filter) =>
            {
                try
                {
                    filter ??= new EmployeeFilter();
                    var pagedEmployees = await employeeService.GetPagedEmployeesAsync(filter);
                    var response = BaseResponse<PagedDto<EmployeeResponseDto>>.Success(pagedEmployees);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<PagedDto<EmployeeResponseDto>>.Failure("An error occurred while retrieving employees.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get Employee By Id
            employeeGroup.MapGet("/{id:guid}", async (IEmployeeService employeeService, string id) =>
            {
                try
                {
                    var employee = await employeeService.GetEmployeeByIdAsync(id);
                    if (employee == null)
                    {
                        var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("Employee not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<EmployeeResponseDto>.Success(employee));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("An error occurred while retrieving the employee.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Create Employee
            employeeGroup.MapPost("/", async (IEmployeeService employeeService, [FromBody] EmployeeRequestDto createEmployeeDto) =>
            {
                if (createEmployeeDto == null)
                {
                    var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("Employee data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var employee = await employeeService.CreateEmployeeAsync(createEmployeeDto);
                    return Results.Ok(BaseResponse<EmployeeResponseDto>.Success(employee));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<EmployeeResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("An error occurred while creating the employee.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Update Employee
            employeeGroup.MapPut("/{id:guid}", async (IEmployeeService employeeService, string id, [FromBody] EmployeeRequestDto updateEmployeeDto) =>
            {
                if (updateEmployeeDto == null)
                {
                    var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("Employee data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var updatedEmployee = await employeeService.UpdateEmployeeAsync(id, updateEmployeeDto);
                    return Results.Ok(BaseResponse<EmployeeResponseDto>.Success(updatedEmployee));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<EmployeeResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("An error occurred while updating the employee.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete Employee
            employeeGroup.MapDelete("/{id:guid}", async (IEmployeeService employeeService, string id) =>
            {
                try
                {
                    var isDeleted = await employeeService.DeleteEmployeeAsync(id);
                    if (!isDeleted)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Employee not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while deleting the employee.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
