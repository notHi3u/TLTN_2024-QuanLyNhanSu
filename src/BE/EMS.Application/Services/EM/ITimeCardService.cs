using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface ITimeCardService
    {
        Task<TimeCardResponseDto> GetTimeCardByIdAsync(string id);
        Task<TimeCardResponseDto> CreateTimeCardAsync(TimeCardRequestDto timeCardRequestDto);
        Task<TimeCardResponseDto> UpdateTimeCardAsync(string id, TimeCardRequestDto timeCardRequestDto);
        Task<bool> DeleteTimeCardAsync(string id);
        Task<PagedDto<TimeCardResponseDto>> GetPagedTimeCardsAsync(TimeCardFilter filter);
    }
}
