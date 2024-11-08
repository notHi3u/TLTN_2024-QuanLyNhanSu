using Common.Data;
using Common.Dtos;
using EMS.Domain.Filters.EMS;
using EMS.Domain.Models.EM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Domain.Repositories.EM
{
    public interface IAttendanceRepository: IBaseRepository<Attendance>
    {
        Task<PagedDto<Attendance>> GetPagedAsync(AttendanceFilter filter);
        Task<List<Attendance>?> GetByEmployeeIdAsync( string employeeId );
        Task<List<Attendance>?> GetByTimeCardIdAsync(long timeCardId);
    }
}
