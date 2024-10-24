using AutoMapper;
using EMS.Application.DTOs;
using EMS.Domain.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.Automapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            #region Permission
            CreateMap<PermissionRequestDto, Permission>();
            CreateMap<Permission, PermissionResponseDto>();
            #endregion

            #region Role
            CreateMap<Role, RoleResponseDto>().ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions.Select(ur => ur.Permission)));
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
        }
    }
}
