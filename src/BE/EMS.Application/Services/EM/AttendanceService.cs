using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs.Account;
using EMS.Application.DTOs.EM;
using EMS.Domain.Filters.Account;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.Account;
using EMS.Domain.Models.EM;
using EMS.Domain.Repositories.Account;
using EMS.Domain.Repositories.EM;
using EMS.Infrastructure.Repositories.Account;
using System.Data;

namespace EMS.Application.Services.EM
{
    public class AttendanceService : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly IMapper _mapper;

        public AttendanceService(IAttendanceRepository attendanceRepository, IMapper mapper)
        {
            _attendanceRepository = attendanceRepository ?? throw new ArgumentNullException(nameof(attendanceRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region Create
        public async Task<AttendanceResponseDto> CreateAttendanceAsync(AttendanceRequestDto attendanceRequestDto)
        {
            if(attendanceRequestDto == null)
                throw new ArgumentNullException(nameof(attendanceRequestDto));

            Attendance attendance = _mapper.Map<Attendance>(attendanceRequestDto);
            // Check for existence of a permission with the same name
            if (await _attendanceRepository.ExistsAsync(a => a.EmployeeId == attendance.EmployeeId && a.Date == attendance.Date))
            {
                // You can return null, throw an exception, or return a specific response
                // Here, I'll return a null response and let the calling method handle it
                throw new ArgumentException("Permission already exists"); // or create a specific response indicating a duplicate
            }

            await _attendanceRepository.AddAsync(attendance);
            return _mapper.Map<AttendanceResponseDto>(attendance);
        }
        #endregion

        #region Delete
        public async Task<bool> DeleteAttendanceAsync(long id)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(id);
            if (attendance == null)
            {
                throw new ArgumentNullException(nameof(attendance));
            }

            await _attendanceRepository.DeleteAsync(attendance);
            return true;
        }
        #endregion

        #region Get by Id
        public async Task<AttendanceResponseDto> GetAttendanceByIdAsync(long id)
        {
            var attendance = await _attendanceRepository.GetByIdAsync(id);
            if (attendance == null)
            {
                throw new ArgumentNullException(nameof(attendance));
            }

            return _mapper.Map<AttendanceResponseDto>(attendance);
        }
        #endregion

        #region Get paged
        public async Task<PagedDto<AttendanceResponseDto>> GetPagedAttendancesAsync(AttendanceFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var pagedAttendances = await _attendanceRepository.GetPagedAsync(filter);

            var attendanceResponseDtos = _mapper.Map<IEnumerable<AttendanceResponseDto>>(pagedAttendances.Items);

            var pagedResponse = new PagedDto<AttendanceResponseDto>(
                attendanceResponseDtos,
                pagedAttendances.TotalCount,
                pagedAttendances.PageIndex,
                pagedAttendances.PageSize
            );

            return pagedResponse;
        }
        #endregion

        #region Update
        public async Task<AttendanceResponseDto> UpdateAttendanceAsync(long id, AttendanceRequestDto attendanceRequestDto)
        {
            if (attendanceRequestDto == null)
            {
                throw new ArgumentNullException(nameof(attendanceRequestDto));
            }

            var attendance = await _attendanceRepository.GetByIdAsync(id);
            if (attendance == null)
            {
                throw new ArgumentNullException(nameof(attendance));
            }

            _mapper.Map(attendanceRequestDto, attendance);

            if (await _attendanceRepository.ExistsAsync(a => a.EmployeeId == attendance.EmployeeId && a.Date == attendance.Date))
            {
                throw new ArgumentException("Permission already exists");
            }

            await _attendanceRepository.UpdateAsync(attendance);
            return _mapper.Map<AttendanceResponseDto>(attendance);
        }
        #endregion

        #region Get by EmployeeId
        public async Task<IEnumerable<AttendanceResponseDto>> GetAttendancesByEmployIdAsync(string employeeId)
        {
            var attendance = await _attendanceRepository.GetByEmployeeIdAsync(employeeId);

            if (attendance == null)
                throw new ArgumentNullException(nameof(attendance));

            return _mapper.Map<IEnumerable<AttendanceResponseDto>>(attendance);
        }
        #endregion

        #region Get by TimeCardId
        public async Task<IEnumerable<AttendanceResponseDto>> GetAttendancesByTimeCardIdAsync(long timeCardId)
        {
            var attendance = await _attendanceRepository.GetByTimeCardIdAsync(timeCardId);

            if (attendance == null)
                throw new ArgumentNullException(nameof(attendance));

            return _mapper.Map<IEnumerable<AttendanceResponseDto>>(attendance);
        }
        #endregion

        #region Delete Bulk
        public async Task<int> DeleteBulkAsync(IEnumerable<long> ids)
        {
            if (ids == null || !ids.Any())
            {
                throw new ArgumentException("No attendance IDs provided");
            }

            int deletedCount = 0;

            // Iterate over each ID and fetch the corresponding attendance record
            foreach (var id in ids)
            {
                var attendance = await _attendanceRepository.GetByIdAsync(id);
                if (attendance != null)
                {
                    await _attendanceRepository.DeleteAsync(attendance);
                    deletedCount++; // Increment the deleted count
                }
            }

            return deletedCount; // Return the number of deleted records
        }
        #endregion
    }
}
