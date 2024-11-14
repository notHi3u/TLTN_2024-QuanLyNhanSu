using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;

namespace EMS.Application.Services.EM
{
    public class HolidayLeavePolicyService : IHolidayLeavePolicyService
    {
        private readonly IHolidayLeavePolicyRepository _holidayLeavePolicyRepository;
        private readonly IMapper _mapper;

        public HolidayLeavePolicyService(IHolidayLeavePolicyRepository holidayLeavePolicyRepository, IMapper mapper)
        {
            _holidayLeavePolicyRepository = holidayLeavePolicyRepository ?? throw new ArgumentNullException(nameof(holidayLeavePolicyRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<HolidayLeavePolicyResponseDto> CreateHolidayLeavePolicyAsync(HolidayLeavePolicyRequestDto holidayLeavePolicyRequestDto)
        {
            // Ensure the DTO is not null
            if (holidayLeavePolicyRequestDto == null)
                throw new ArgumentNullException(nameof(holidayLeavePolicyRequestDto));

            // Check if a HolidayLeavePolicy already exists for the given year
            bool policyExists = await _holidayLeavePolicyRepository.ExistsAsync(
                x => x.EffectiveYear == holidayLeavePolicyRequestDto.EffectiveYear);

            if (policyExists)
            {
                throw new InvalidOperationException($"A holiday leave policy already exists for the year {holidayLeavePolicyRequestDto.EffectiveYear}.");
            }

            // Map the request DTO to the HolidayLeavePolicy model
            HolidayLeavePolicy policy = _mapper.Map<HolidayLeavePolicy>(holidayLeavePolicyRequestDto);

            // Calculate the number of holidays
            policy.HolidayCount = policy.Holidays.Count();

            // Add the new policy to the repository
            await _holidayLeavePolicyRepository.AddAsync(policy);

            // Return the response DTO after mapping the policy
            return _mapper.Map<HolidayLeavePolicyResponseDto>(policy);
        }


        public async Task<bool> DeleteHolidayLeavePolicyAsync(int id)
        {
            var policy = await _holidayLeavePolicyRepository.GetByIdAsync(id);
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            await _holidayLeavePolicyRepository.DeleteAsync(policy);
            return true;
        }

        public async Task<HolidayLeavePolicyResponseDto> GetHolidayLeavePolicyByIdAsync(int id)
        {
            var policy = await _holidayLeavePolicyRepository.GetByIdAsync(id);
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            return _mapper.Map<HolidayLeavePolicyResponseDto>(policy);
        }

        public async Task<PagedDto<HolidayLeavePolicyResponseDto>> GetPagedHolidayLeavePoliciesAsync(HolidayLeavePolicyFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedPolicies = await _holidayLeavePolicyRepository.GetPagedAsync(filter);
            var policyDtos = _mapper.Map<IEnumerable<HolidayLeavePolicyResponseDto>>(pagedPolicies.Items);

            return new PagedDto<HolidayLeavePolicyResponseDto>(
                policyDtos,
                pagedPolicies.TotalCount,
                pagedPolicies.PageIndex,
                pagedPolicies.PageSize
            );
        }

        public async Task<HolidayLeavePolicyResponseDto> UpdateHolidayLeavePolicyAsync(int id, HolidayLeavePolicyRequestDto holidayLeavePolicyRequestDto)
        {
            if (holidayLeavePolicyRequestDto == null)
                throw new ArgumentNullException(nameof(holidayLeavePolicyRequestDto));

            var policy = await _holidayLeavePolicyRepository.GetByIdAsync(id);
            if (policy == null)
                throw new ArgumentNullException(nameof(policy));

            _mapper.Map(holidayLeavePolicyRequestDto, policy);
            await _holidayLeavePolicyRepository.UpdateAsync(policy);

            return _mapper.Map<HolidayLeavePolicyResponseDto>(policy);
        }
    }
}
