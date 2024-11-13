using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;

namespace EMS.Application.Services.EM
{
    public class WorkRecordService : IWorkRecordService
    {
        private readonly IWorkRecordRepository _workHistoryRepository;
        private readonly IMapper _mapper;

        public WorkRecordService(IWorkRecordRepository workHistoryRepository, IMapper mapper)
        {
            _workHistoryRepository = workHistoryRepository ?? throw new ArgumentNullException(nameof(workHistoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<WorkRecordResponseDto> CreateWorkHistoryAsync(WorkRecordRequestDto workHistoryRequestDto)
        {
            if (workHistoryRequestDto == null)
                throw new ArgumentNullException(nameof(workHistoryRequestDto));

            WorkRecord workHistory = _mapper.Map<WorkRecord>(workHistoryRequestDto);
            await _workHistoryRepository.AddAsync(workHistory);
            return _mapper.Map<WorkRecordResponseDto>(workHistory);
        }

        public async Task<bool> DeleteWorkHistoryAsync(long id)
        {
            var workHistory = await _workHistoryRepository.GetByIdAsync(id);
            if (workHistory == null)
                throw new ArgumentNullException(nameof(workHistory));

            await _workHistoryRepository.DeleteAsync(workHistory);
            return true;
        }

        public async Task<WorkRecordResponseDto> GetWorkHistoryByIdAsync(long id)
        {
            var workHistory = await _workHistoryRepository.GetByIdAsync(id);
            if (workHistory == null)
                throw new ArgumentNullException(nameof(workHistory));

            return _mapper.Map<WorkRecordResponseDto>(workHistory);
        }

        public async Task<PagedDto<WorkRecordResponseDto>> GetPagedWorkHistoriesAsync(WorkRecordFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedWorkHistories = await _workHistoryRepository.GetPagedAsync(filter);
            var workHistoryDtos = _mapper.Map<IEnumerable<WorkRecordResponseDto>>(pagedWorkHistories.Items);

            return new PagedDto<WorkRecordResponseDto>(
                workHistoryDtos,
                pagedWorkHistories.TotalCount,
                pagedWorkHistories.PageIndex,
                pagedWorkHistories.PageSize
            );
        }

        public async Task<WorkRecordResponseDto> UpdateWorkHistoryAsync(long id, WorkRecordRequestDto workHistoryRequestDto)
        {
            if (workHistoryRequestDto == null)
                throw new ArgumentNullException(nameof(workHistoryRequestDto));

            var workHistory = await _workHistoryRepository.GetByIdAsync(id);
            if (workHistory == null)
                throw new ArgumentNullException(nameof(workHistory));

            _mapper.Map(workHistoryRequestDto, workHistory);
            await _workHistoryRepository.UpdateAsync(workHistory);

            return _mapper.Map<WorkRecordResponseDto>(workHistory);
        }
    }
}
