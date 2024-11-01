using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.EM;

namespace EMS.Application.Services.EM
{
    public class EmployeeRelativeService : IEmployeeRelativeService
    {
        private readonly IEmployeeRelativeRepository _employeeRelativeRepository;
        private readonly IMapper _mapper;

        public EmployeeRelativeService(IEmployeeRelativeRepository employeeRelativeRepository, IMapper mapper)
        {
            _employeeRelativeRepository = employeeRelativeRepository ?? throw new ArgumentNullException(nameof(employeeRelativeRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<EmployeeRelativeResponseDto> CreateEmployeeRelativeAsync(EmployeeRelativeRequestDto employeeRelativeRequestDto)
        {
            if (employeeRelativeRequestDto == null)
                throw new ArgumentNullException(nameof(employeeRelativeRequestDto));

            EmployeeRelative relative = _mapper.Map<EmployeeRelative>(employeeRelativeRequestDto);
            await _employeeRelativeRepository.AddAsync(relative);
            return _mapper.Map<EmployeeRelativeResponseDto>(relative);
        }

        public async Task<bool> DeleteEmployeeRelativeAsync(string id)
        {
            var relative = await _employeeRelativeRepository.GetByIdAsync(id);
            if (relative == null)
                throw new ArgumentNullException(nameof(relative));

            await _employeeRelativeRepository.DeleteAsync(relative);
            return true;
        }

        public async Task<EmployeeRelativeResponseDto> GetEmployeeRelativeByIdAsync(string id)
        {
            var relative = await _employeeRelativeRepository.GetByIdAsync(id);
            if (relative == null)
                throw new ArgumentNullException(nameof(relative));

            return _mapper.Map<EmployeeRelativeResponseDto>(relative);
        }

        public async Task<PagedDto<EmployeeRelativeResponseDto>> GetPagedEmployeeRelativesAsync(EmployeeRelativeFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            var pagedRelatives = await _employeeRelativeRepository.GetPagedAsync(filter);
            var relativeDtos = _mapper.Map<IEnumerable<EmployeeRelativeResponseDto>>(pagedRelatives.Items);

            return new PagedDto<EmployeeRelativeResponseDto>(
                relativeDtos,
                pagedRelatives.TotalCount,
                pagedRelatives.PageIndex,
                pagedRelatives.PageSize
            );
        }

        public async Task<EmployeeRelativeResponseDto> UpdateEmployeeRelativeAsync(string id, EmployeeRelativeRequestDto employeeRelativeRequestDto)
        {
            if (employeeRelativeRequestDto == null)
                throw new ArgumentNullException(nameof(employeeRelativeRequestDto));

            var relative = await _employeeRelativeRepository.GetByIdAsync(id);
            if (relative == null)
                throw new ArgumentNullException(nameof(relative));

            _mapper.Map(employeeRelativeRequestDto, relative);
            await _employeeRelativeRepository.UpdateAsync(relative);

            return _mapper.Map<EmployeeRelativeResponseDto>(relative);
        }
    }
}
