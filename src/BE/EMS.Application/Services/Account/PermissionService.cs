using AutoMapper;
using Common.Dtos;
using EMS.Application.DTOs;
using EMS.Domain.Filters.Account;
using EMS.Domain.Models.Account;
using EMS.Domain.Repositories.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.Services.Account
{
    public class PermissionService : IPermissionService
    {

        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;

        public PermissionService(IPermissionRepository permissionRepository, IMapper mapper)
        {
            _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        #region Create

        public async Task<PermissionResponseDto> CreatePermissionAsync(PermissionRequestDto permissionRequestDto)
        {
            if (permissionRequestDto == null)
            {
                throw new ArgumentNullException(nameof(permissionRequestDto));
            }

            var permission = _mapper.Map<Permission>(permissionRequestDto);
            permission.Id = Guid.NewGuid().ToString();
            await _permissionRepository.AddAsync(permission);
            return _mapper.Map<PermissionResponseDto>(permission);
        }

        #endregion

        #region Delete

        public async Task<bool> DeletePermissionAsync(string id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null)
            {
                return false; // or throw an exception if preferred
            }

            await _permissionRepository.DeleteAsync(permission);
            return true;
        }

        #endregion

        #region GetPaged

        public async Task<PagedDto<PermissionResponseDto>> GetPagedPermissionsAsync(PermissionFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var pagedPermissions = await _permissionRepository.GetPagedAsync(filter);

            var permissionResponseDtos = _mapper.Map<IEnumerable<PermissionResponseDto>>(pagedPermissions.Items);

            var pagedResponse = new PagedDto<PermissionResponseDto>(
                permissionResponseDtos,
                pagedPermissions.TotalCount,
                pagedPermissions.PageIndex,
                pagedPermissions.PageSize
            );

            return pagedResponse;
        }

        #endregion

        #region GetById

        public async Task<PermissionResponseDto> GetPermissionByIdAsync(string id)
        {
            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null)
            {
                return null; // or throw an exception if preferred
            }

            return _mapper.Map<PermissionResponseDto>(permission);
        }

        #endregion

        #region Update

        public async Task<PermissionResponseDto> UpdatePermissionAsync(string id, PermissionRequestDto permissionRequestDto)
        {
            if (permissionRequestDto == null)
            {
                throw new ArgumentNullException(nameof(permissionRequestDto));
            }

            var permission = await _permissionRepository.GetByIdAsync(id);
            if (permission == null)
            {
                return null; // or throw an exception if preferred
            }

            _mapper.Map(permissionRequestDto, permission);
            await _permissionRepository.UpdateAsync(permission);
            return _mapper.Map<PermissionResponseDto>(permission);
        }

        #endregion
    }
}
