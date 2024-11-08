using Common.Configurations;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Application.Services.EM;
using Microsoft.AspNetCore.Mvc;

namespace EMS.API.Endpoints.EM
{
    public static class TimeCardEndpoints
    {
        public static void Map(WebApplication app)
        {
            var timeCardGroup = app.MapGroup("/timecards")
                .WithTags("TimeCards");

            //#region Get TimeCards By Employee Id
            //timeCardGroup.MapGet("/employee/{employeeId:guid}", async (ITimeCardService timeCardService, string employeeId, DateTime? startDate, DateTime? endDate) =>
            //{
            //    try
            //    {
            //        var timeCards = await timeCardService.GetTimeCardsByEmployeeIdAsync(employeeId, startDate, endDate);
            //        if (timeCards == null || !timeCards.Any())
            //        {
            //            var errorResponse = BaseResponse<IEnumerable<TimeCardResponseDto>>.Failure("Time cards not found for the specified employee.");
            //            return Results.NotFound(errorResponse);
            //        }
            //        return Results.Ok(BaseResponse<IEnumerable<TimeCardResponseDto>>.Success(timeCards));
            //    }
            //    catch (Exception ex)
            //    {
            //        var errorResponse = BaseResponse<IEnumerable<TimeCardResponseDto>>.Failure("An error occurred while retrieving time cards.");
            //        return Results.Problem(detail: errorResponse.Errors[0], statusCode: errorResponse.StatusCode);
            //    }
            //}).ConfigureApiResponses();
            //#endregion

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
