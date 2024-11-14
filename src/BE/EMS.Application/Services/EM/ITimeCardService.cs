using Common.Dtos;
using Common.Enums;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;

namespace EMS.Application.Services.EM
{
    public interface ITimeCardService
    {
        Task<TimeCardResponseDto> GetTimeCardByIdAsync(long id, bool? isDeep);
        Task<TimeCardResponseDto> CreateTimeCardAsync(TimeCardRequestDto timeCardRequestDto);
        Task<TimeCardResponseDto> UpdateTimeCardAsync(long id, TimeCardRequestDto timeCardRequestDto);
        Task<bool> DeleteTimeCardAsync(long id);
        Task<PagedDto<TimeCardResponseDto>> GetPagedTimeCardsAsync(TimeCardFilter filter);
        Task<TimeCardResponseDto> ChangeTimeCardStatus(long id, TimeCardStatus timeCardStatus);
    }
}
