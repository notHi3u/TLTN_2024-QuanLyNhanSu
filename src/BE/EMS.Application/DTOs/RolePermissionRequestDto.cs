using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.DTOs
{
    public class RolePermissionRequestDto
    {
        public required string RoleId { get; set; }
        public required string PermissionId { get; set; }
    }
}
