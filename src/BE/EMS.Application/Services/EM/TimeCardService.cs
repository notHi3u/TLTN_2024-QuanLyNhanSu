using AutoMapper;
using Common.Dtos;
using Common.Enums;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;

namespace EMS.Application.Services.EM
{
    public class TimeCardService : ITimeCardService
    {
        private readonly ITimeCardRepository _timeCardRepository;
        private readonly IMapper _mapper;

        public TimeCardService(ITimeCardRepository timeCardRepository, IMapper mapper)
        {
            _timeCardRepository = timeCardRepository ?? throw new ArgumentNullException(nameof(timeCardRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<TimeCardResponseDto> CreateTimeCardAsync(TimeCardRequestDto timeCardRequestDto)
        {
            if (timeCardRequestDto == null)
                throw new ArgumentNullException(nameof(timeCardRequestDto));

            TimeCard timeCard = _mapper.Map<TimeCard>(timeCardRequestDto);
            await _timeCardRepository.AddAsync(timeCard);
            return _mapper.Map<TimeCardResponseDto>(timeCard);
        }

        public async Task<bool> DeleteTimeCardAsync(long id)
        {
            var timeCard = await _timeCardRepository.GetByIdAsync(id);
            if (timeCard == null)
                throw new ArgumentNullException(nameof(timeCard));

            await _timeCardRepository.DeleteAsync(timeCard);
            return true;
        }

        public async Task<TimeCardResponseDto> GetTimeCardByIdAsync(long id, bool? isDeep)
        {
            var timeCard = await _timeCardRepository.GetByIdAsync(id, isDeep);
            if (timeCard == null)
                throw new ArgumentNullException(nameof(timeCard));

            return _mapper.Map<TimeCardResponseDto>(timeCard);
        }

        public async Task<PagedDto<TimeCardResponseDto>> GetPagedTimeCardsAsync(TimeCardFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedTimeCards = await _timeCardRepository.GetPagedAsync(filter);
            var timeCardDtos = _mapper.Map<IEnumerable<TimeCardResponseDto>>(pagedTimeCards.Items);

            return new PagedDto<TimeCardResponseDto>(
                timeCardDtos,
                pagedTimeCards.TotalCount,
                pagedTimeCards.PageIndex,
                pagedTimeCards.PageSize
            );
        }

        public async Task<TimeCardResponseDto> UpdateTimeCardAsync(long id, TimeCardRequestDto timeCardRequestDto)
        {
            if (timeCardRequestDto == null)
                throw new ArgumentNullException(nameof(timeCardRequestDto));

            var timeCard = await _timeCardRepository.GetByIdAsync(id);
            if (timeCard == null)
                throw new ArgumentNullException(nameof(timeCard));

            _mapper.Map(timeCardRequestDto, timeCard);
            await _timeCardRepository.UpdateAsync(timeCard);

            return _mapper.Map<TimeCardResponseDto>(timeCard);
        }

        public async Task<TimeCardStatus> ChangeTimeCardStatus(long id, TimeCardStatus timeCardStatus)
        {
            var timeCard = await _timeCardRepository.GetByIdAsync(id);
            if (timeCard == null) 
            {
                throw new ArgumentNullException(nameof(timeCard));
            }

            timeCard.Status = timeCardStatus;
            await _timeCardRepository.UpdateAsync(timeCard);

            return (TimeCardStatus)timeCard.Status;
        }
    }
}
