using Microsoft.AspNetCore.Authorization;

public class DepartmentManagerRequirement : IAuthorizationRequirement
{
    public string DepartmentId { get; }

    public DepartmentManagerRequirement(string departmentId)
    {
        DepartmentId = departmentId;
    }
}