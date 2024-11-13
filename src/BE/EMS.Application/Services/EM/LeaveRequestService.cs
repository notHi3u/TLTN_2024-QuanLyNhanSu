using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;

namespace EMS.Application.Services.EM
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;

        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<LeaveRequestResponseDto> CreateLeaveRequestAsync(LeaveRequestRequestDto leaveRequestDto)
        {
            if (leaveRequestDto == null)
                throw new ArgumentNullException(nameof(leaveRequestDto));

            LeaveRequest leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestDto);
            await _leaveRequestRepository.AddAsync(leaveRequest);
            return _mapper.Map<LeaveRequestResponseDto>(leaveRequest);
        }

        public async Task<bool> DeleteLeaveRequestAsync(long id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
                throw new ArgumentNullException(nameof(leaveRequest));

            await _leaveRequestRepository.DeleteAsync(leaveRequest);
            return true;
        }

        public async Task<LeaveRequestResponseDto> GetLeaveRequestByIdAsync(long id)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
                throw new ArgumentNullException(nameof(leaveRequest));

            return _mapper.Map<LeaveRequestResponseDto>(leaveRequest);
        }

        public async Task<PagedDto<LeaveRequestResponseDto>> GetPagedLeaveRequestsAsync(LeaveRequestFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedLeaveRequests = await _leaveRequestRepository.GetPagedAsync(filter);
            var leaveRequestDtos = _mapper.Map<IEnumerable<LeaveRequestResponseDto>>(pagedLeaveRequests.Items);

            return new PagedDto<LeaveRequestResponseDto>(
                leaveRequestDtos,
                pagedLeaveRequests.TotalCount,
                pagedLeaveRequests.PageIndex,
                pagedLeaveRequests.PageSize
            );
        }

        public async Task<LeaveRequestResponseDto> UpdateLeaveRequestAsync(long id, LeaveRequestRequestDto leaveRequestDto)
        {
            if (leaveRequestDto == null)
                throw new ArgumentNullException(nameof(leaveRequestDto));

            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
                throw new ArgumentNullException(nameof(leaveRequest));

            _mapper.Map(leaveRequestDto, leaveRequest);
            await _leaveRequestRepository.UpdateAsync(leaveRequest);

            return _mapper.Map<LeaveRequestResponseDto>(leaveRequest);
        }
    }
}
