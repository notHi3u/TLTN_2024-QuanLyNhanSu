using EMS.Application.DTOs;
using EMS.Domain.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.Services.Account
{
    public interface ITfaService
    {
        Task<TwoFactorAuthSetupInfoDto> LoadSharedKeyAndQrCodeUriAsync(User user);
        //Task<BaseResponse<bool>> Verify2faToken(User user, string verificationCode);
    }
}
