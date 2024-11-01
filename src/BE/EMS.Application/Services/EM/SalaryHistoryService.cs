using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;

namespace EMS.Application.Services.EM
{
    public class SalaryHistoryService : ISalaryHistoryService
    {
        private readonly ISalaryHistoryRepository _salaryHistoryRepository;
        private readonly IMapper _mapper;

        public SalaryHistoryService(ISalaryHistoryRepository salaryHistoryRepository, IMapper mapper)
        {
            _salaryHistoryRepository = salaryHistoryRepository ?? throw new ArgumentNullException(nameof(salaryHistoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SalaryHistoryResponseDto> CreateSalaryHistoryAsync(SalaryHistoryRequestDto salaryHistoryRequestDto)
        {
            if (salaryHistoryRequestDto == null)
                throw new ArgumentNullException(nameof(salaryHistoryRequestDto));

            SalaryHistory salaryHistory = _mapper.Map<SalaryHistory>(salaryHistoryRequestDto);
            await _salaryHistoryRepository.AddAsync(salaryHistory);
            return _mapper.Map<SalaryHistoryResponseDto>(salaryHistory);
        }

        public async Task<bool> DeleteSalaryHistoryAsync(string id)
        {
            var salaryHistory = await _salaryHistoryRepository.GetByIdAsync(id);
            if (salaryHistory == null)
                throw new ArgumentNullException(nameof(salaryHistory));

            await _salaryHistoryRepository.DeleteAsync(salaryHistory);
            return true;
        }

        public async Task<SalaryHistoryResponseDto> GetSalaryHistoryByIdAsync(string id)
        {
            var salaryHistory = await _salaryHistoryRepository.GetByIdAsync(id);
            if (salaryHistory == null)
                throw new ArgumentNullException(nameof(salaryHistory));

            return _mapper.Map<SalaryHistoryResponseDto>(salaryHistory);
        }

        public async Task<PagedDto<SalaryHistoryResponseDto>> GetPagedSalaryHistoriesAsync(SalaryHistoryFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedHistories = await _salaryHistoryRepository.GetPagedAsync(filter);
            var historyDtos = _mapper.Map<IEnumerable<SalaryHistoryResponseDto>>(pagedHistories.Items);

            return new PagedDto<SalaryHistoryResponseDto>(
                historyDtos,
                pagedHistories.TotalCount,
                pagedHistories.PageIndex,
                pagedHistories.PageSize
            );
        }

        public async Task<SalaryHistoryResponseDto> UpdateSalaryHistoryAsync(string id, SalaryHistoryRequestDto salaryHistoryRequestDto)
        {
            if (salaryHistoryRequestDto == null)
                throw new ArgumentNullException(nameof(salaryHistoryRequestDto));

            var salaryHistory = await _salaryHistoryRepository.GetByIdAsync(id);
            if (salaryHistory == null)
                throw new ArgumentNullException(nameof(salaryHistory));

            _mapper.Map(salaryHistoryRequestDto, salaryHistory);
            await _salaryHistoryRepository.UpdateAsync(salaryHistory);

            return _mapper.Map<SalaryHistoryResponseDto>(salaryHistory);
        }
    }
}
