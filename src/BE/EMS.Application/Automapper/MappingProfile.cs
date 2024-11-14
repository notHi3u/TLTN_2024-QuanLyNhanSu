using AutoMapper;
using Common.Enums;
using EMS.Application.DTOs.Account;
using EMS.Application.DTOs.EM;
using EMS.Domain.Models.Account;
using EMS.Domain.Models.EM;
using System.Linq;

namespace EMS.Application.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Permission
            CreateMap<PermissionRequestDto, Permission>();
            CreateMap<Permission, PermissionResponseDto>();
            #endregion

            #region Role
            CreateMap<Role, RoleResponseDto>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions.Select(rp => rp.Permission)));
            CreateMap<RoleRequestDto, Role>();
            #endregion

            #region RolePermission
            CreateMap<RolePermission, RolePermissionResponseDto>();
            #endregion

            #region User
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role)));
            CreateMap<UserRequestDto, User>();
            #endregion

            #region Attendance
            CreateMap<AttendanceRequestDto, Attendance>();
            CreateMap<Attendance, AttendanceResponseDto>();
            #endregion

            #region Employee
            CreateMap<EmployeeRequestDto, Employee>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString())) // Set EmployeeId to a new GUID
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status ?? EmployeeStatus.Active)) // Set default status if null
                ;
            CreateMap<Employee, EmployeeResponseDto>()
                .ForMember(dest => dest.Department, opt => opt.Ignore()) // Ignore Department property
                .ForMember(dest => dest.ManagedDepartment, opt => opt.Ignore()) // Ignore ManagedDepartment property
                .ForMember(dest => dest.TimeCards, opt => opt.MapFrom(src => src.TimeCards)) // Mapping for TimeCards
                .ForMember(dest => dest.LeaveRequests, opt => opt.MapFrom(src => src.LeaveRequests)) // Mapping for LeaveRequests
                .ForMember(dest => dest.LeaveBalances, opt => opt.MapFrom(src => src.LeaveBalances)) // Mapping for LeaveBalances
                .ForMember(dest => dest.Attendances, opt => opt.MapFrom(src => src.Attendances)) // Mapping for Attendances
                .ForMember(dest => dest.EmployeeRelatives, opt => opt.MapFrom(src => src.EmployeeRelatives)) // Mapping for EmployeeRelatives
                .ForMember(dest => dest.WorkRecord, opt => opt.MapFrom(src => src.WorkRecord)); // Mapping for WorkRecord

            #endregion

            #region LeaveRequest
            CreateMap<LeaveRequestRequestDto, LeaveRequest>();
            CreateMap<LeaveRequest, LeaveRequestResponseDto>();
            #endregion

            #region LeaveBalance
            CreateMap<LeaveBalanceRequestDto, LeaveBalance>();
            CreateMap<LeaveBalance, LeaveBalanceResponseDto>();
            #endregion

            #region SalaryRecord
            CreateMap<SalaryRecordRequestDto, SalaryRecord>();
            CreateMap<SalaryRecord, SalaryRecordResponseDto>();
            #endregion

            #region TimeCard
            CreateMap<TimeCardRequestDto, TimeCard>();
            CreateMap<TimeCard, TimeCardResponseDto>()
                .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee)) // Map Employee
                .ForMember(dest => dest.Attendances, opt => opt.MapFrom(src => src.Attendances));
            #endregion

            #region WorkHistory
            CreateMap<WorkRecordRequestDto, WorkRecord>();
            CreateMap<WorkRecord, WorkRecordResponseDto>();
            #endregion

            #region Department
            CreateMap<DepartmentRequestDto, Department>();
            CreateMap<Department, DepartmentResponseDto>()
                .ForMember(dest => dest.Employees, opt => opt.MapFrom(src => src.Employees)) // Mapping for Employees
                .ForMember(dest => dest.Manager, opt => opt.MapFrom(src => src.Manager)); // Mapping for Manager
            #endregion


            #region HolidayLeavePolicy
            CreateMap<HolidayLeavePolicyRequestDto, HolidayLeavePolicy>();
            CreateMap<HolidayLeavePolicy, HolidayLeavePolicyResponseDto>();
            #endregion

            #region EmployeeRelative
            CreateMap<EmployeeRelativeRequestDto, EmployeeRelative>();
            CreateMap<EmployeeRelative, EmployeeRelativeResponseDto>();
            #endregion
        }
    }
}
