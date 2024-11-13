using EMS.Domain.Models.Account;
using EMS.Domain.Models.EM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EMS.Infrastructure.Contexts
{
    public class AppDbContext : IdentityDbContext<
        User, Role, string,
        IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSets for Account-related entities
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        // DbSets for Employee Management-related entities
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<EmployeeRelative> EmployeeRelatives { get; set; }
        public DbSet<SalaryRecord> SalaryRecords { get; set; }
        public DbSet<TimeCard> TimeCards { get; set; }
        public DbSet<WorkRecord> WorkHistories { get; set; }
        public DbSet<HolidayLeavePolicy> HolidayLeavePolicies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Account Entity Configurations

            // Configure User and Employee relationship (1-1)
            builder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure Role and UserRole relationship (1-n)
            builder.Entity<Role>()
                .HasMany(r => r.UserRoles)
                .WithOne(ur => ur.Role)
                .HasForeignKey(ur => ur.RoleId);

            // Configure User and UserRole relationship (1-n)
            builder.Entity<User>()
                .HasMany(u => u.UserRoles)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId);

            // Configure Role and Permission relationship (n-m via RolePermission)
            builder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure RefreshToken entity
            builder.Entity<RefreshToken>()
                .HasKey(rt => rt.Id);

            #endregion

            #region Employee Management (EM) Entity Configurations

            // Configure Employee and Department relationship (n-1)
            builder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            builder.Entity<Department>()
                .HasOne(d=> d.Manager)  // A department has one manager (employee)
                .WithOne(e => e.ManagedDepartment)  // The employee has exactly one department manager (1-1)
                .HasForeignKey<Department>(d => d.DepartmentManagerId);  // The DepartmentManagerId in Department is the FK



            // Configure Employee and Attendance relationship (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.Attendances)
                .WithOne(a => a.Employee)
                .HasForeignKey(a => a.EmployeeId);

            // Configure Employee and LeaveRequest relationship (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.LeaveRequests)
                .WithOne(lr => lr.Employee)
                .HasForeignKey(lr => lr.EmployeeId);

            // Configure Employee and LeaveBalance relationship (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.LeaveBalances)
                .WithOne(lb => lb.Employee)
                .HasForeignKey(lb => lb.EmployeeId);

            // Configure Employee and EmployeeRelative relationship (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.EmployeeRelatives)
                .WithOne(er => er.Employee)
                .HasForeignKey(er => er.EmployeeId);


            // Configure Employee and SalaryHistory relationship (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.SalaryRecords)
                .WithOne(sh => sh.Employee)
                .HasForeignKey(sh => sh.EmployeeId);

            // Configure Employee and TimeCard relationship (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.TimeCards)
                .WithOne(tc => tc.Employee)
                .HasForeignKey(tc => tc.EmployeeId);

            // Configure Employee and WorkHistory relationship (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.WorkRecord)
                .WithOne(wh => wh.Employee)
                .HasForeignKey(wh => wh.EmployeeId);

            #endregion

            #region Seeding

            #endregion
        }
    }
}
