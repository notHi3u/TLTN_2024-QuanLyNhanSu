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
        private readonly IWorkRecordRepository _workRecordRepository; // Changed "workHistoryRepository" to "workRecordRepository"
        private readonly IMapper _mapper;

        public WorkRecordService(IWorkRecordRepository workRecordRepository, IMapper mapper) // Updated constructor parameter
        {
            _workRecordRepository = workRecordRepository ?? throw new ArgumentNullException(nameof(workRecordRepository)); // Updated variable name
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<WorkRecordResponseDto> CreateWorkRecordAsync(WorkRecordRequestDto workRecordRequestDto) // Changed method name to "CreateWorkRecordAsync"
        {
            if (workRecordRequestDto == null)
                throw new ArgumentNullException(nameof(workRecordRequestDto));

            WorkRecord workRecord = _mapper.Map<WorkRecord>(workRecordRequestDto); // Changed "workHistory" to "workRecord"
            await _workRecordRepository.AddAsync(workRecord); // Updated repository method
            return _mapper.Map<WorkRecordResponseDto>(workRecord);
        }

        public async Task<bool> DeleteWorkRecordAsync(long id) // Changed method name to "DeleteWorkRecordAsync"
        {
            var workRecord = await _workRecordRepository.GetByIdAsync(id); // Updated variable name
            if (workRecord == null)
                throw new ArgumentNullException(nameof(workRecord));

            await _workRecordRepository.DeleteAsync(workRecord); // Updated repository method
            return true;
        }

        public async Task<WorkRecordResponseDto> GetWorkRecordByIdAsync(long id) // Changed method name to "GetWorkRecordByIdAsync"
        {
            var workRecord = await _workRecordRepository.GetByIdAsync(id); // Updated variable name
            if (workRecord == null)
                throw new ArgumentNullException(nameof(workRecord));

            return _mapper.Map<WorkRecordResponseDto>(workRecord);
        }

        public async Task<PagedDto<WorkRecordResponseDto>> GetPagedWorkRecordsAsync(WorkRecordFilter filter) // Changed method name to "GetPagedWorkRecordsAsync"
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedWorkRecords = await _workRecordRepository.GetPagedAsync(filter); // Updated variable name
            var workRecordDtos = _mapper.Map<IEnumerable<WorkRecordResponseDto>>(pagedWorkRecords.Items); // Updated variable name

            return new PagedDto<WorkRecordResponseDto>(
                workRecordDtos,
                pagedWorkRecords.TotalCount,
                pagedWorkRecords.PageIndex,
                pagedWorkRecords.PageSize
            );
        }

        public async Task<WorkRecordResponseDto> UpdateWorkRecordAsync(long id, WorkRecordRequestDto workRecordRequestDto) // Changed method name to "UpdateWorkRecordAsync"
        {
            if (workRecordRequestDto == null)
                throw new ArgumentNullException(nameof(workRecordRequestDto));

            var workRecord = await _workRecordRepository.GetByIdAsync(id); // Updated variable name
            if (workRecord == null)
                throw new ArgumentNullException(nameof(workRecord));

            _mapper.Map(workRecordRequestDto, workRecord);
            await _workRecordRepository.UpdateAsync(workRecord);

            return _mapper.Map<WorkRecordResponseDto>(workRecord);
        }
    }
}
