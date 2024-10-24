using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.DTOs
{
    public class RecoveryCodesResponseDto
    {
        public string[]? RecoveryCodes { get; set; }
        public int RecoveryCodesLeft { get; set; }
    }
}
