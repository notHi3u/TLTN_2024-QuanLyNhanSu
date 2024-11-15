using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.Account;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.Account;
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
            employeeGroup.MapGet("/{id}", async (IEmployeeService employeeService, string id) =>
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
            employeeGroup.MapPost("/", async (IEmployeeService employeeService, IUserService userService, [FromBody] EmployeeRequestDto createEmployeeDto) =>
            {
                if (createEmployeeDto == null)
                {
                    var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("Employee data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var employee = await employeeService.CreateEmployeeAsync(createEmployeeDto);
                    var password = await userService.GenerateInitPassword();

                    var user = new UserRequestDto { Email = employee.Email, Password = password };
                    var createdUser = await userService.CreateUserAsync(user);

                    await employeeService.BindUserToEmployeeAsync(employee.Id, createdUser.Data.Id);
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
            employeeGroup.MapPut("/{id}", async (IEmployeeService employeeService, string id, [FromBody] EmployeeRequestDto updateEmployeeDto) =>
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
            employeeGroup.MapDelete("/{id}", async (IEmployeeService employeeService, string id) =>
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

            #region Assign Department to Employee
            employeeGroup.MapPut("/{id}/assign-department", async (IEmployeeService employeeService, string id, [FromBody] string departmentId) =>
            {
                if (departmentId == null || string.IsNullOrEmpty(departmentId))
                {
                    var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("Department ID is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    // Attempt to assign the department to the employee
                    var updatedEmployee = await employeeService.AssignDepartmentAsync(id, departmentId);

                    if (updatedEmployee == null)
                    {
                        var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("Employee or Department not found.");
                        return Results.NotFound(errorResponse);
                    }

                    return Results.Ok(BaseResponse<bool>.Success(updatedEmployee));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("An error occurred while assigning the department.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Remove Department from Employee
            employeeGroup.MapPut("/{id}/remove-department", async (IEmployeeService employeeService, string id) =>
            {
                try
                {
                    // Attempt to remove the department from the employee
                    var updatedEmployee = await employeeService.RemoveDepartmentAsync(id);

                    if (updatedEmployee == null)
                    {
                        var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("Employee not found.");
                        return Results.NotFound(errorResponse);
                    }

                    return Results.Ok(BaseResponse<bool>.Success(updatedEmployee));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<EmployeeResponseDto>.Failure("An error occurred while removing the department.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
