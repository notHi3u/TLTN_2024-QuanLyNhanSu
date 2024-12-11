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
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Set EmployeeId to a new GUID
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status ?? EmployeeStatus.Active)) // Set default status if null
                .ForMember(dest => dest.UserId, opt => opt.Ignore()) // UserId will be set later
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<Employee, EmployeeResponseDto>()
                // Ignoring complex navigation properties if they are not needed in the response.
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department)) // Map Department if needed
                .ForMember(dest => dest.ManagedDepartment, opt => opt.MapFrom(src => src.ManagedDepartment)) // Map ManagedDepartment if needed
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User)) // Map the User entity (if User is part of Employee)
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))

                // Mapping and calculating NetSalary based on BaseSalary, Bonuses, and Deductions
                .ForMember(dest => dest.NetSalary, opt => opt.MapFrom(src => src.BaseSalary + src.Bonuses - src.Deductions));

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
                .ForMember(dest => dest.Manager, opt => opt.MapFrom(src => src.Manager)) // Mapping for Manager
                .ForMember(dest => dest.TotalSalary, opt => opt.MapFrom(src => src.TotalSalary));
            #endregion

            #region HolidayLeavePolicy
            CreateMap<HolidayLeavePolicyRequestDto, HolidayLeavePolicy>();
            CreateMap<HolidayLeavePolicy, HolidayLeavePolicyResponseDto>();
            #endregion

            #region EmployeeRelative
            CreateMap<EmployeeRelativeRequestDto, EmployeeRelative>();
            CreateMap<EmployeeRelative, EmployeeRelativeResponseDto>();
            #endregion

            #region Employee - SalaryRecord
            CreateMap<Employee, SalaryRecord>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Id)) // Mapping EmployeeId
                .ForMember(dest => dest.BaseSalary, opt => opt.MapFrom(src => src.BaseSalary)) // Map BaseSalary
                .ForMember(dest => dest.Bonuses, opt => opt.MapFrom(src => src.Bonuses)) // Map Bonuses
                .ForMember(dest => dest.Deductions, opt => opt.MapFrom(src => src.Deductions)) // Map Deductions
                .ForMember(dest => dest.NetSalary, opt => opt.MapFrom(src => src.BaseSalary + src.Bonuses - src.Deductions)); // Calculate NetSalary
            #endregion
        }
    }
}
