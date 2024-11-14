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
        private readonly ILeaveBalanceRepository _balanceRepository;

        // Constructor for dependency injection
        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, IMapper mapper, ILeaveBalanceRepository balanceRepository)
        {
            _leaveRequestRepository = leaveRequestRepository ?? throw new ArgumentNullException(nameof(leaveRequestRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _balanceRepository = balanceRepository ?? throw new ArgumentNullException(nameof(balanceRepository));
        }

        // Create a new leave request
        public async Task<LeaveRequestResponseDto> CreateLeaveRequestAsync(LeaveRequestRequestDto leaveRequestDto)
        {
            // Validate that the StartDate is before or equal to the EndDate
            if (leaveRequestDto.StartDate.CompareTo(leaveRequestDto.EndDate) > 0)
            {
                throw new ArgumentException("EndDate cannot be earlier than StartDate.");
            }

            // Ensure the leaveRequestDto is not null
            if (leaveRequestDto == null)
                throw new ArgumentNullException(nameof(leaveRequestDto));

            // Check if the employee's leave balance exists, if not, create one
            if (!await _balanceRepository.BalanceExists(leaveRequestDto.EmployeeId))
            {
                var leaveBalance = new LeaveBalance
                {
                    EmployeeId = leaveRequestDto.EmployeeId,
                    Year = DateTime.Now.Year,
                };
                await _balanceRepository.AddAsync(leaveBalance);
            }

            // Map the DTO to the LeaveRequest model
            LeaveRequest leaveRequest = _mapper.Map<LeaveRequest>(leaveRequestDto);

            // Retrieve the current leave balance for the employee
            var balance = await _balanceRepository.GetByEmployeeIdAndNow(leaveRequest.EmployeeId);

            // Calculate the number of leave days requested, including the start date
            int leaveDays = (leaveRequestDto.EndDate.ToDateTime(TimeOnly.MinValue) - leaveRequestDto.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1;
            balance.UsedLeaveDays += leaveDays; // Update the used leave days

            // Save the leave request and update the balance
            await _leaveRequestRepository.AddAsync(leaveRequest);
            await _balanceRepository.UpdateAsync(balance);

            // Return the response DTO after mapping the leave request
            return _mapper.Map<LeaveRequestResponseDto>(leaveRequest);
        }

        // Delete a leave request by its ID
        public async Task<bool> DeleteLeaveRequestAsync(long id)
        {
            // Get the leave request by ID
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
                throw new ArgumentNullException(nameof(leaveRequest));

            // Delete the leave request
            await _leaveRequestRepository.DeleteAsync(leaveRequest);
            return true; // Indicate successful deletion
        }

        // Get a leave request by its ID
        public async Task<LeaveRequestResponseDto> GetLeaveRequestByIdAsync(long id)
        {
            // Retrieve the leave request by ID
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
                throw new ArgumentNullException(nameof(leaveRequest));

            // Map and return the leave request as a response DTO
            return _mapper.Map<LeaveRequestResponseDto>(leaveRequest);
        }

        // Get a paged list of leave requests based on the filter
        public async Task<PagedDto<LeaveRequestResponseDto>> GetPagedLeaveRequestsAsync(LeaveRequestFilter filter)
        {
            // Ensure the filter is not null
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            // Get the paged leave requests from the repository
            var pagedLeaveRequests = await _leaveRequestRepository.GetPagedAsync(filter);

            // Map the leave request models to DTOs
            var leaveRequestDtos = _mapper.Map<IEnumerable<LeaveRequestResponseDto>>(pagedLeaveRequests.Items);

            // Return the paged DTO result
            return new PagedDto<LeaveRequestResponseDto>(
                leaveRequestDtos,
                pagedLeaveRequests.TotalCount,
                pagedLeaveRequests.PageIndex,
                pagedLeaveRequests.PageSize
            );
        }

        // Update an existing leave request by ID
        public async Task<LeaveRequestResponseDto> UpdateLeaveRequestAsync(long id, LeaveRequestRequestDto leaveRequestDto)
        {
            // Ensure the DTO is not null
            if (leaveRequestDto == null)
                throw new ArgumentNullException(nameof(leaveRequestDto));

            // Get the existing leave request by ID
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(id);
            if (leaveRequest == null)
                throw new ArgumentNullException(nameof(leaveRequest));

            // Calculate the original and new leave days
            int originalLeaveDays = (leaveRequest.EndDate.ToDateTime(TimeOnly.MinValue) - leaveRequest.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1;
            int updatedLeaveDays = (leaveRequestDto.EndDate.ToDateTime(TimeOnly.MinValue) - leaveRequestDto.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1;

            // Map the updated leave request data from the DTO to the existing leave request
            _mapper.Map(leaveRequestDto, leaveRequest);

            // Retrieve the current leave balance for the employee
            var balance = await _balanceRepository.GetByEmployeeIdAndNow(leaveRequest.EmployeeId);

            // Calculate the balance difference based on the updated and original leave days
            int leaveDaysDifference = updatedLeaveDays - originalLeaveDays;

            // Update the balance to reflect the change in used leave days
            balance.UsedLeaveDays += leaveDaysDifference;

            // Save the updated leave request and update the balance
            await _leaveRequestRepository.UpdateAsync(leaveRequest);
            await _balanceRepository.UpdateAsync(balance);

            // Return the updated leave request as a response DTO
            return _mapper.Map<LeaveRequestResponseDto>(leaveRequest);
        }

    }
}
