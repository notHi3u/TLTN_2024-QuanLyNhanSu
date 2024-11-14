using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using EMS.Domain.Filters.EMS;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class TimeCardEndpoints
    {
        public static void Map(WebApplication app)
        {
            var timeCardGroup = app.MapGroup("/timecards")
                .WithTags("TimeCards");

            #region Get TimeCards By Filter
            timeCardGroup.MapGet("/", async (ITimeCardService timeCardService, [AsParameters] TimeCardFilter filter) =>
            {
                try
                {
                    filter ??= new TimeCardFilter();
                    var pagedTimeCards = await timeCardService.GetPagedTimeCardsAsync(filter);
                    var response = BaseResponse<PagedDto<TimeCardResponseDto>>.Success(pagedTimeCards);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<PagedDto<TimeCardResponseDto>>.Failure("An error occurred while retrieving leave requests.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Get TimeCards By Id
            timeCardGroup.MapGet("/{id}", async (ITimeCardService timeCardService, long id, bool? isDeep) =>
            {
                try
                {
                    var timeCard = await timeCardService.GetTimeCardByIdAsync(id, isDeep);
                    if (timeCard == null)
                    {
                        var errorResponse = BaseResponse<TimeCardResponseDto>.Failure("Salary history not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<TimeCardResponseDto>.Success(timeCard));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<SalaryRecordResponseDto>.Failure("An error occurred while retrieving salary history.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Create TimeCard
            timeCardGroup.MapPost("/", async (ITimeCardService timeCardService, [FromBody] TimeCardRequestDto createTimeCardDto) =>
            {
                if (createTimeCardDto == null)
                {
                    var errorResponse = BaseResponse<TimeCardResponseDto>.Failure("Time card data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var timeCard = await timeCardService.CreateTimeCardAsync(createTimeCardDto);
                    return Results.Ok(BaseResponse<TimeCardResponseDto>.Success(timeCard));
                }
                catch (ArgumentException ex)
                {
                    var errorResponse = BaseResponse<TimeCardResponseDto>.Failure(ex.Message);
                    return Results.BadRequest(errorResponse);
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<TimeCardResponseDto>.Failure("An error occurred while creating the time card.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Update TimeCard
            timeCardGroup.MapPut("/{id:guid}", async (ITimeCardService timeCardService, long id, [FromBody] TimeCardRequestDto updateTimeCardDto) =>
            {
                if (updateTimeCardDto == null)
                {
                    var errorResponse = BaseResponse<TimeCardResponseDto>.Failure("Time card data is required.");
                    return Results.BadRequest(errorResponse);
                }

                try
                {
                    var updatedTimeCard = await timeCardService.UpdateTimeCardAsync(id, updateTimeCardDto);
                    if (updatedTimeCard == null)
                    {
                        var errorResponse = BaseResponse<TimeCardResponseDto>.Failure("Time card not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.Ok(BaseResponse<TimeCardResponseDto>.Success(updatedTimeCard));
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<TimeCardResponseDto>.Failure("An error occurred while updating the time card.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion

            #region Delete TimeCard
            timeCardGroup.MapDelete("/{id:guid}", async (ITimeCardService timeCardService, long id) =>
            {
                try
                {
                    var isDeleted = await timeCardService.DeleteTimeCardAsync(id);
                    if (!isDeleted)
                    {
                        var errorResponse = BaseResponse<bool>.Failure("Time card not found.");
                        return Results.NotFound(errorResponse);
                    }
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    var errorResponse = BaseResponse<bool>.Failure("An error occurred while deleting the time card.");
                    return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
                }
            }).ConfigureApiResponses();
            #endregion
        }
    }
}
