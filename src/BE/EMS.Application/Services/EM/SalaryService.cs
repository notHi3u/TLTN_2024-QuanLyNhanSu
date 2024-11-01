using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;

namespace EMS.Application.Services.EM
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepository _salaryRepository;
        private readonly IMapper _mapper;

        public SalaryService(ISalaryRepository salaryRepository, IMapper mapper)
        {
            _salaryRepository = salaryRepository ?? throw new ArgumentNullException(nameof(salaryRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SalaryResponseDto> CreateSalaryAsync(SalaryRequestDto salaryRequestDto)
        {
            if (salaryRequestDto == null)
                throw new ArgumentNullException(nameof(salaryRequestDto));

            Salary salary = _mapper.Map<Salary>(salaryRequestDto);
            await _salaryRepository.AddAsync(salary);
            return _mapper.Map<SalaryResponseDto>(salary);
        }

        public async Task<bool> DeleteSalaryAsync(string id)
        {
            var salary = await _salaryRepository.GetByIdAsync(id);
            if (salary == null)
                throw new ArgumentNullException(nameof(salary));

            await _salaryRepository.DeleteAsync(salary);
            return true;
        }

        public async Task<SalaryResponseDto> GetSalaryByIdAsync(string id)
        {
            var salary = await _salaryRepository.GetByIdAsync(id);
            if (salary == null)
                throw new ArgumentNullException(nameof(salary));

            return _mapper.Map<SalaryResponseDto>(salary);
        }

        public async Task<PagedDto<SalaryResponseDto>> GetPagedSalariesAsync(SalaryFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedSalaries = await _salaryRepository.GetPagedAsync(filter);
            var salaryDtos = _mapper.Map<IEnumerable<SalaryResponseDto>>(pagedSalaries.Items);

            return new PagedDto<SalaryResponseDto>(
                salaryDtos,
                pagedSalaries.TotalCount,
                pagedSalaries.PageIndex,
                pagedSalaries.PageSize
            );
        }

        public async Task<SalaryResponseDto> UpdateSalaryAsync(string id, SalaryRequestDto salaryRequestDto)
        {
            if (salaryRequestDto == null)
                throw new ArgumentNullException(nameof(salaryRequestDto));

            var salary = await _salaryRepository.GetByIdAsync(id);
            if (salary == null)
                throw new ArgumentNullException(nameof(salary));

            _mapper.Map(salaryRequestDto, salary);
            await _salaryRepository.UpdateAsync(salary);

            return _mapper.Map<SalaryResponseDto>(salary);
        }
    }
}
