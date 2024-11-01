using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;

namespace EMS.Application.Services.EM
{
    public class WorkHistoryService : IWorkHistoryService
    {
        private readonly IWorkHistoryRepository _workHistoryRepository;
        private readonly IMapper _mapper;

        public WorkHistoryService(IWorkHistoryRepository workHistoryRepository, IMapper mapper)
        {
            _workHistoryRepository = workHistoryRepository ?? throw new ArgumentNullException(nameof(workHistoryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<WorkHistoryResponseDto> CreateWorkHistoryAsync(WorkHistoryRequestDto workHistoryRequestDto)
        {
            if (workHistoryRequestDto == null)
                throw new ArgumentNullException(nameof(workHistoryRequestDto));

            WorkHistory workHistory = _mapper.Map<WorkHistory>(workHistoryRequestDto);
            await _workHistoryRepository.AddAsync(workHistory);
            return _mapper.Map<WorkHistoryResponseDto>(workHistory);
        }

        public async Task<bool> DeleteWorkHistoryAsync(string id)
        {
            var workHistory = await _workHistoryRepository.GetByIdAsync(id);
            if (workHistory == null)
                throw new ArgumentNullException(nameof(workHistory));

            await _workHistoryRepository.DeleteAsync(workHistory);
            return true;
        }

        public async Task<WorkHistoryResponseDto> GetWorkHistoryByIdAsync(string id)
        {
            var workHistory = await _workHistoryRepository.GetByIdAsync(id);
            if (workHistory == null)
                throw new ArgumentNullException(nameof(workHistory));

            return _mapper.Map<WorkHistoryResponseDto>(workHistory);
        }

        public async Task<PagedDto<WorkHistoryResponseDto>> GetPagedWorkHistoriesAsync(WorkHistoryFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedWorkHistories = await _workHistoryRepository.GetPagedAsync(filter);
            var workHistoryDtos = _mapper.Map<IEnumerable<WorkHistoryResponseDto>>(pagedWorkHistories.Items);

            return new PagedDto<WorkHistoryResponseDto>(
                workHistoryDtos,
                pagedWorkHistories.TotalCount,
                pagedWorkHistories.PageIndex,
                pagedWorkHistories.PageSize
            );
        }

        public async Task<WorkHistoryResponseDto> UpdateWorkHistoryAsync(string id, WorkHistoryRequestDto workHistoryRequestDto)
        {
            if (workHistoryRequestDto == null)
                throw new ArgumentNullException(nameof(workHistoryRequestDto));

            var workHistory = await _workHistoryRepository.GetByIdAsync(id);
            if (workHistory == null)
                throw new ArgumentNullException(nameof(workHistory));

            _mapper.Map(workHistoryRequestDto, workHistory);
            await _workHistoryRepository.UpdateAsync(workHistory);

            return _mapper.Map<WorkHistoryResponseDto>(workHistory);
        }
    }
}
