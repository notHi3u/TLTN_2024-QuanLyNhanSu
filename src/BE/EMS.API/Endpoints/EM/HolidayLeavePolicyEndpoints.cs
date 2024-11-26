using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using EMS.Domain.Filters.EMS;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class HolidayLeavePolicyEndpoints
    {
        public static void Map(WebApplication app)
        {
            var policyGroup = app.MapGroup("/holiday-leave-policies")
                .WithTags("Holiday Leave Policy")
                .RequireAuthorization();

            #region Get All Holiday Leave Policies
            policyGroup.MapGet("/", async (IHolidayLeavePolicyService policyService, [AsParameters] HolidayLeavePolicyFilter filter) =>
            {
                try
                {
                    filter ??= new HolidayLeavePolicyFilter();
                    var pagedPolicies = await policyService.GetPagedHolidayLeavePoliciesAsync(filter);
                    var response = BaseResponse<PagedDto<HolidayLeavePolicyResponseDto>>.Success(pagedPolicies);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<PagedDto<HolidayLeavePolicyResponseDto>>.Failure("An error occurred while retrieving holiday leave policies.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get Holiday Leave Policy By Id
            policyGroup.MapGet("/{id}", async (IHolidayLeavePolicyService policyService, int id) =>
            {
                try
                {
                    var policy = await policyService.GetHolidayLeavePolicyByIdAsync(id);
                    if (policy == null)
                    {
                        var errorResponse = BaseResponse<HolidayLeavePolicyResponseDto>.Failure("Holiday leave policy not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<HolidayLeavePolicyResponseDto>.Success(policy));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<HolidayLeavePolicyResponseDto>.Failure("An error occurred while retrieving the holiday leave policy.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Create Holiday Leave Policy
            policyGroup.MapPost("/", async (IHolidayLeavePolicyService policyService, [FromBody] HolidayLeavePolicyRequestDto createPolicyDto) =>
            {
                if (createPolicyDto == null)
                {
                    var errorResponse = BaseResponse<HolidayLeavePolicyResponseDto>.Failure("Holiday leave policy data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var policy = await policyService.CreateHolidayLeavePolicyAsync(createPolicyDto);
                    return Results.Ok(BaseResponse<HolidayLeavePolicyResponseDto>.Success(policy));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<HolidayLeavePolicyResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<HolidayLeavePolicyResponseDto>.Failure("An error occurred while creating the holiday leave policy.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Update Holiday Leave Policy
            policyGroup.MapPut("/{id}", async (IHolidayLeavePolicyService policyService, int id, [FromBody] HolidayLeavePolicyRequestDto updatePolicyDto) =>
            {
                if (updatePolicyDto == null)
                {
                    var errorResponse = BaseResponse<HolidayLeavePolicyResponseDto>.Failure("Holiday leave policy data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var updatedPolicy = await policyService.UpdateHolidayLeavePolicyAsync(id, updatePolicyDto);
                    return Results.Ok(BaseResponse<HolidayLeavePolicyResponseDto>.Success(updatedPolicy));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<HolidayLeavePolicyResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<HolidayLeavePolicyResponseDto>.Failure("An error occurred while updating the holiday leave policy.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete Holiday Leave Policy
            policyGroup.MapDelete("/{id}", async (IHolidayLeavePolicyService policyService, int id) =>
            {
                try
                {
                    var isDeleted = await policyService.DeleteHolidayLeavePolicyAsync(id);
                    if (!isDeleted)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Holiday leave policy not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while deleting the holiday leave policy.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
