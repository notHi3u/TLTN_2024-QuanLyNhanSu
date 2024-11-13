using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;

namespace EMS.Application.Services.EM
{
    public class SalaryRecordService : ISalaryRecordService
    {
        private readonly ISalaryRecordRepository _salaryHistoryRepository;
        private readonly IMapper _mapper;

        public SalaryRecordService(ISalaryRecordRepository salaryHistoryRepository, IMapper mapper)
        {
            _salaryHistoryRepository = salaryHistoryRepository ?? throw new ArgumentNullException(nameof(salaryHistoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SalaryRecordResponseDto> CreateSalaryHistoryAsync(SalaryRecordRequestDto salaryHistoryRequestDto)
        {
            if (salaryHistoryRequestDto == null)
                throw new ArgumentNullException(nameof(salaryHistoryRequestDto));

            SalaryRecord salaryRecord = _mapper.Map<SalaryRecord>(salaryHistoryRequestDto);
            await _salaryHistoryRepository.AddAsync(salaryRecord);
            return _mapper.Map<SalaryRecordResponseDto>(salaryRecord);
        }

        public async Task<bool> DeleteSalaryHistoryAsync(long id)
        {
            var salaryHistory = await _salaryHistoryRepository.GetByIdAsync(id);
            if (salaryHistory == null)
                throw new ArgumentNullException(nameof(salaryHistory));

            await _salaryHistoryRepository.DeleteAsync(salaryHistory);
            return true;
        }

        public async Task<SalaryRecordResponseDto> GetSalaryHistoryByIdAsync(long id)
        {
            var salaryHistory = await _salaryHistoryRepository.GetByIdAsync(id);
            if (salaryHistory == null)
                throw new ArgumentNullException(nameof(salaryHistory));

            return _mapper.Map<SalaryRecordResponseDto>(salaryHistory);
        }

        public async Task<PagedDto<SalaryRecordResponseDto>> GetPagedSalaryHistoriesAsync(SalaryRecordFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedHistories = await _salaryHistoryRepository.GetPagedAsync(filter);
            var historyDtos = _mapper.Map<IEnumerable<SalaryRecordResponseDto>>(pagedHistories.Items);

            return new PagedDto<SalaryRecordResponseDto>(
                historyDtos,
                pagedHistories.TotalCount,
                pagedHistories.PageIndex,
                pagedHistories.PageSize
            );
        }

        public async Task<SalaryRecordResponseDto> UpdateSalaryHistoryAsync(long id, SalaryRecordRequestDto salaryHistoryRequestDto)
        {
            if (salaryHistoryRequestDto == null)
                throw new ArgumentNullException(nameof(salaryHistoryRequestDto));

            var salaryHistory = await _salaryHistoryRepository.GetByIdAsync(id);
            if (salaryHistory == null)
                throw new ArgumentNullException(nameof(salaryHistory));

            _mapper.Map(salaryHistoryRequestDto, salaryHistory);
            await _salaryHistoryRepository.UpdateAsync(salaryHistory);

            return _mapper.Map<SalaryRecordResponseDto>(salaryHistory);
        }

        public async Task<ICollection<SalaryRecordResponseDto>> GetSalaryHistoryByEmployeeIdAsync(string id)
        {
            var salaryRecords = await _salaryHistoryRepository.GetByEmployeeId(id);

            return _mapper.Map<ICollection<SalaryRecordResponseDto>>(salaryRecords);
        }
    }
}
