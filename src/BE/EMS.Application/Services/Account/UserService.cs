﻿using AutoMapper;
using Common.Dtos;
using Common.Helpers;
using EMS.Application.DTOs.Account;
using EMS.Domain.Filters.Account;
using EMS.Domain.Models.Account;
using EMS.Domain.Repositories.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Security.Cryptography;
using System.Text;

namespace EMS.Application.Services.Account
{
    public class UserService : IUserService
    {
        #region Constructor
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IMailHelper _mailHelper;
        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            UserManager<User> userManager,
            IEmailSender emailSender,
            IMailHelper mailHelper
            )
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _mailHelper = mailHelper ?? throw new ArgumentNullException(nameof(mailHelper));
        }
        #endregion

        #region Get User by Id

        public async Task<BaseResponse<UserResponseDto>> GetUserByIdAsync(string id, bool? isDeep = false)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BaseResponse<UserResponseDto>.Failure("User ID cannot be null or empty.");
            }

            var user = await _userRepository.GetByIdAsync(id, isDeep);
            if (user == null)
            {
                return BaseResponse<UserResponseDto>.Failure("User not found.");
            }

            var userResponseDto = _mapper.Map<UserResponseDto>(user);
            return BaseResponse<UserResponseDto>.Success(userResponseDto);
        }

        #endregion

        #region Create User

        public async Task<BaseResponse<UserResponseDto>> CreateUserAsync(UserRequestDto userRequestDto)
        {
            if (userRequestDto == null)
            {
                return BaseResponse<UserResponseDto>.Failure("User request cannot be null.");
            }

            var user = _mapper.Map<User>(userRequestDto);
            user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user, userRequestDto.Password);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodeToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var activationLink = $"http://localhost:5202/Auth/ConfirmEmail?userId={user.Id}&code={encodeToken}";

                // Gửi email chào mừng với liên kết kích hoạt
                await _emailSender.SendWelcomeEmailAsync(user.Email, user.UserName, activationLink, userRequestDto.Password);

                // Trả về phản hồi thành công với dữ liệu người dùng đã tạo
                var userResponseDto = _mapper.Map<UserResponseDto>(user);
                return BaseResponse<UserResponseDto>.Success(userResponseDto);
            }
            else
            {
                // Xử lý lỗi từ IdentityResult
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BaseResponse<UserResponseDto>.Failure(errors);
            }
        }

        #endregion

        #region Update User

        public async Task<BaseResponse<UserResponseDto>> UpdateUserAsync(string id, UserRequestDto userRequestDto)
        {
            if (userRequestDto == null)
            {
                return BaseResponse<UserResponseDto>.Failure("User request cannot be null.");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BaseResponse<UserResponseDto>.Failure("User not found.");
            }

            // Ánh xạ thông tin từ DTO vào đối tượng người dùng
            _mapper.Map(userRequestDto, user);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Trả về phản hồi thành công với dữ liệu người dùng đã cập nhật
                var userResponseDto = _mapper.Map<UserResponseDto>(user);
                return BaseResponse<UserResponseDto>.Success(userResponseDto);
            }
            else
            {
                // Xử lý lỗi từ IdentityResult
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BaseResponse<UserResponseDto>.Failure(errors);
            }
        }

        #endregion

        #region Delete User

        public async Task<BaseResponse<bool>> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return BaseResponse<bool>.Failure("User not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return BaseResponse<bool>.Success(true);
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BaseResponse<bool>.Failure(errors);
            }
        }

        #endregion

        #region Get Paged Users
        public async Task<PagedDto<UserResponseDto>> GetPagedUsersAsync(UserFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            // Khởi tạo giá trị mặc định nếu cần
            filter.PageIndex = filter.PageIndex ?? 1;
            filter.PageSize = filter.PageSize ?? 10;

            // Retrieve paged users from repository
            var userPage = await _userRepository.GetPagedAsync(filter);

            // Map the User entities to UserResponseDto
            var userResponseDtos = _mapper.Map<IEnumerable<UserResponseDto>>(userPage.Items);

            // Create the paged response for UserResponseDto
            var pagedResponse = new PagedDto<UserResponseDto>(
                userResponseDtos,
                userPage.TotalCount,
                filter.PageIndex.Value,
                filter.PageSize.Value
            );

            return pagedResponse;
        }

        #endregion

        #region Assign Role to User

        public async Task<BaseResponse<bool>> AssignRoleToUserAsync(string userId, string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleName))
            {
                return BaseResponse<bool>.Failure("User ID and Role Name cannot be null or empty.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BaseResponse<bool>.Failure("User not found.");
            }

            // Kiểm tra xem vai trò đã tồn tại chưa
            var roleExists = await _userManager.IsInRoleAsync(user, roleName);
            if (roleExists)
            {
                return BaseResponse<bool>.Failure("User already has this role.");
            }

            // Gán vai trò cho người dùng
            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return BaseResponse<bool>.Success(true);
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BaseResponse<bool>.Failure(errors);
            }
        }

        #endregion

        #region Assign Roles to User

        public async Task<BaseResponse<bool>> AssignRolesToUserAsync(string userId, IEnumerable<string> roleNames)
        {
            if (string.IsNullOrWhiteSpace(userId) || roleNames == null || !roleNames.Any())
            {
                return BaseResponse<bool>.Failure("User ID and Role Names cannot be null or empty.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BaseResponse<bool>.Failure("User not found.");
            }

            // Kiểm tra xem tất cả các vai trò có tồn tại không
            var validRoleNames = new List<string>();
            foreach (var roleName in roleNames)
            {
                if (await _userManager.IsInRoleAsync(user, roleName) == false)
                {
                    validRoleNames.Add(roleName);
                }
            }

            if (!validRoleNames.Any())
            {
                return BaseResponse<bool>.Failure("No valid roles to assign.");
            }

            // Gán tất cả các vai trò cho người dùng
            var result = await _userManager.AddToRolesAsync(user, validRoleNames);

            if (result.Succeeded)
            {
                return BaseResponse<bool>.Success(true);
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BaseResponse<bool>.Failure(errors);
            }
        }

        #endregion

        #region Remove Role from User

        public async Task<BaseResponse<bool>> RemoveRoleFromUserAsync(string userId, string roleName)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(roleName))
            {
                return BaseResponse<bool>.Failure("User ID and Role Name cannot be null or empty.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BaseResponse<bool>.Failure("User not found.");
            }

            // Kiểm tra xem người dùng có vai trò này không
            var roleExists = await _userManager.IsInRoleAsync(user, roleName);
            if (!roleExists)
            {
                return BaseResponse<bool>.Failure("User does not have this role.");
            }

            // Xóa vai trò khỏi người dùng
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (result.Succeeded)
            {
                return BaseResponse<bool>.Success(true);
            }
            else
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BaseResponse<bool>.Failure(errors);
            }
        }

        #endregion

        #region Remove Roles from User

        public async Task<BaseResponse<bool>> RemoveRolesFromUserAsync(string userId, IEnumerable<string> roleNames)
        {
            if (string.IsNullOrWhiteSpace(userId) || roleNames == null || !roleNames.Any())
            {
                return BaseResponse<bool>.Failure("User ID cannot be null or empty, and at least one role name must be provided.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BaseResponse<bool>.Failure("User not found.");
            }

            var errors = new List<string>();

            foreach (var roleName in roleNames)
            {
                // Check if the user is in this role
                if (!await _userManager.IsInRoleAsync(user, roleName))
                {
                    errors.Add($"User is not in role '{roleName}'.");
                    continue; // Skip to the next role
                }

                // Remove the role from the user
                var result = await _userManager.RemoveFromRoleAsync(user, roleName);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors.Select(e => e.Description));
                }
            }

            if (errors.Any())
            {
                return BaseResponse<bool>.Failure(errors);
            }

            return BaseResponse<bool>.Success(true);
        }

        #endregion

        #region Update User Roles

        public async Task<BaseResponse<bool>> UpdateUserRolesAsync(string userId, IEnumerable<string> roleNames)
        {
            if (string.IsNullOrWhiteSpace(userId) || roleNames == null || !roleNames.Any())
            {
                return BaseResponse<bool>.Failure("User ID and Role Names cannot be null or empty.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BaseResponse<bool>.Failure("User not found.");
            }

            // Lấy tất cả các vai trò hiện tại của người dùng
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Xóa tất cả các vai trò hiện tại
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                var errors = removeResult.Errors.Select(e => e.Description).ToList();
                return BaseResponse<bool>.Failure(errors);
            }

            // Gán các vai trò mới
            var addResult = await _userManager.AddToRolesAsync(user, roleNames);
            if (addResult.Succeeded)
            {
                return BaseResponse<bool>.Success(true);
            }
            else
            {
                var errors = addResult.Errors.Select(e => e.Description).ToList();
                return BaseResponse<bool>.Failure(errors);
            }
        }

        #endregion

        public async Task<string> GenerateInitPassword()
        {
            return await Task.Run(() => GeneratePassword(12)); // Default length of 12 characters
        }

        public string GeneratePassword(int length)
        {
            if (length < 6) // Minimum length for ASP.NET Identity default policy
                throw new ArgumentException("Password length must be at least 6.", nameof(length));

            // Character sets for different password requirements
            const string upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()-_+=<>?";
            const string allChars = upperChars + lowerChars + digits + specialChars;

            var password = new StringBuilder(length);
            using (var rng = RandomNumberGenerator.Create())
            {
                // Ensure password meets complexity requirements
                password.Append(GetRandomChar(upperChars, rng));
                password.Append(GetRandomChar(lowerChars, rng));
                password.Append(GetRandomChar(digits, rng));
                password.Append(GetRandomChar(specialChars, rng));

                // Fill the rest of the password with random characters from all sets
                while (password.Length < length)
                {
                    password.Append(GetRandomChar(allChars, rng));
                }

                // Shuffle the characters to avoid predictable patterns
                return ShuffleString(password.ToString(), rng);
            }
        }

        private char GetRandomChar(string charSet, RandomNumberGenerator rng)
        {
            var buffer = new byte[1];
            rng.GetBytes(buffer);
            return charSet[buffer[0] % charSet.Length];
        }

        private string ShuffleString(string input, RandomNumberGenerator rng)
        {
            var array = input.ToCharArray();
            for (int i = array.Length - 1; i > 0; i--)
            {
                var buffer = new byte[1];
                rng.GetBytes(buffer);
                int j = buffer[0] % (i + 1);

                // Swap array[i] with array[j]
                (array[i], array[j]) = (array[j], array[i]);
            }
            return new string(array);
        }
    }
}
