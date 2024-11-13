using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;

namespace EMS.Application.Services.EM
{
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
        private readonly IMapper _mapper;

        public LeaveBalanceService(ILeaveBalanceRepository leaveBalanceRepository, IMapper mapper)
        {
            _leaveBalanceRepository = leaveBalanceRepository ?? throw new ArgumentNullException(nameof(leaveBalanceRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<LeaveBalanceResponseDto> CreateLeaveBalanceAsync(LeaveBalanceRequestDto leaveBalanceRequestDto)
        {
            if (leaveBalanceRequestDto == null)
                throw new ArgumentNullException(nameof(leaveBalanceRequestDto));

            LeaveBalance leaveBalance = _mapper.Map<LeaveBalance>(leaveBalanceRequestDto);
            await _leaveBalanceRepository.AddAsync(leaveBalance);
            return _mapper.Map<LeaveBalanceResponseDto>(leaveBalance);
        }

        public async Task<bool> DeleteLeaveBalanceAsync(long id)
        {
            var leaveBalance = await _leaveBalanceRepository.GetByIdAsync(id);
            if (leaveBalance == null)
                throw new ArgumentNullException(nameof(leaveBalance));

            await _leaveBalanceRepository.DeleteAsync(leaveBalance);
            return true;
        }

        public async Task<LeaveBalanceResponseDto> GetLeaveBalanceByIdAsync(long id)
        {
            var leaveBalance = await _leaveBalanceRepository.GetByIdAsync(id);
            if (leaveBalance == null)
                throw new ArgumentNullException(nameof(leaveBalance));

            return _mapper.Map<LeaveBalanceResponseDto>(leaveBalance);
        }

        public async Task<PagedDto<LeaveBalanceResponseDto>> GetPagedLeaveBalancesAsync(LeaveBalanceFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedLeaveBalances = await _leaveBalanceRepository.GetPagedAsync(filter);
            var leaveBalanceDtos = _mapper.Map<IEnumerable<LeaveBalanceResponseDto>>(pagedLeaveBalances.Items);

            return new PagedDto<LeaveBalanceResponseDto>(
                leaveBalanceDtos,
                pagedLeaveBalances.TotalCount,
                pagedLeaveBalances.PageIndex,
                pagedLeaveBalances.PageSize
            );
        }

        public async Task<LeaveBalanceResponseDto> UpdateLeaveBalanceAsync(long id, LeaveBalanceRequestDto leaveBalanceRequestDto)
        {
            if (leaveBalanceRequestDto == null)
                throw new ArgumentNullException(nameof(leaveBalanceRequestDto));

            var leaveBalance = await _leaveBalanceRepository.GetByIdAsync(id);
            if (leaveBalance == null)
                throw new ArgumentNullException(nameof(leaveBalance));

            _mapper.Map(leaveBalanceRequestDto, leaveBalance);
            await _leaveBalanceRepository.UpdateAsync(leaveBalance);

            return _mapper.Map<LeaveBalanceResponseDto>(leaveBalance);
        }
    }
}
