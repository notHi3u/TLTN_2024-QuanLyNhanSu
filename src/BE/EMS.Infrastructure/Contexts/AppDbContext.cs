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
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<SalaryHistory> SalaryHistories { get; set; }
        public DbSet<TimeCard> TimeCards { get; set; }
        public DbSet<WorkHistory> WorkHistories { get; set; }
        public DbSet<HolidayLeavePolicy> HolidayLeavePolicies { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Account-related entity configurations

            builder.Entity<User>(b =>
            {
                b.Property(e => e.PhoneNumber)
                    .HasMaxLength(20);
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            // User and Employee (1-1)
            builder.Entity<User>()
                .HasOne(u => u.Employee)
                .WithOne(e => e.User)
                .HasForeignKey<Employee>(e => e.Id) // Assuming Employee.Id matches User.Id
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Role>(b =>
            {
                b.Property(r => r.Description)
                    .HasMaxLength(250);
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            builder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId }); // Composite primary key

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

            builder.Entity<RefreshToken>().HasKey(t => t.Id);

            #endregion

            #region Employee Management-related entity configurations

            // Department and Employee (1-n)
            builder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId);

            // Department and Manager (1-1)
            builder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithMany() // The Manager is not related back to the department in this context
                .HasForeignKey(d => d.DepartmentManagerId); // Foreign key in Department

            // Employee and Attendance (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.Attendances)
                .WithOne(a => a.Employee)
                .HasForeignKey(a => a.EmpployeeId);

            // Employee and LeaveRequest (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.LeaveRequests)
                .WithOne(l => l.Employee)
                .HasForeignKey(l => l.EmployeeId);

            // Employee and LeaveBalance (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.LeaveBalances)
                .WithOne(lb => lb.Employee)
                .HasForeignKey(lb => lb.EmployeeId);

            // Employee and EmployeeRelative (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.EmployeeRelatives)
                .WithOne(er => er.Employee)
                .HasForeignKey(er => er.EmployeeId);

            // Employee and Salary (1-1)
            builder.Entity<Employee>()
                .HasOne(e => e.Salary)
                .WithOne(s => s.Employee)
                .HasForeignKey<Salary>(s => s.EmployeeId);

            // Employee and SalaryHistory (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.SalaryHistory)
                .WithOne(sh => sh.Employee)
                .HasForeignKey(sh => sh.EmployeeId);

            // Employee and TimeCard (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.TimeCards)
                .WithOne(tc => tc.Employee)
                .HasForeignKey(tc => tc.EmployeeId);

            // Employee and WorkHistory (1-n)
            builder.Entity<Employee>()
                .HasMany(e => e.WorkHistories)
                .WithOne(wh => wh.Employee)
                .HasForeignKey(wh => wh.EmployeeId);

            // TimeCard and Attendance (1-n)
            builder.Entity<TimeCard>()
                .HasMany(tc => tc.Attendances) // A TimeCard can have many Attendances
                .WithOne(a => a.TimeCard) // Each Attendance is associated with one TimeCard
                .HasForeignKey(a => a.TimeCardId);

            // Additional configurations for entities
            builder.Entity<Department>()
                .HasKey(d => d.Id);

            builder.Entity<Employee>()
                .HasKey(e => e.Id);

            builder.Entity<EmployeeRelative>()
                .HasKey(er => er.Id);

            builder.Entity<EmployeeRelative>()
                .Property(er => er.PhoneNumber)
                .HasMaxLength(15); // Example constraint

            builder.Entity<Salary>()
                .HasKey(s => s.Id);

            builder.Entity<Salary>()
                .Property(s => s.BaseSalary)
                .HasColumnType("decimal(18,2)"); // Specify decimal precision

            builder.Entity<SalaryHistory>()
                .HasKey(e => e.Id);

            builder.Entity<SalaryHistory>()
                .Property(sh => sh.BaseSalary)
                .HasColumnType("decimal(18,2)"); // Specify decimal precision

            builder.Entity<TimeCard>()
                .HasKey(e => e.Id);

            builder.Entity<TimeCard>()
                .Property(tc => tc.Status)
                .HasDefaultValue(false); // Default status

            // HolidayLeavePolicy configuration
            builder.Entity<HolidayLeavePolicy>()
                .Property(hp => hp.HolidayCount)
                .IsRequired(); // Make this field required


            builder.HasDefaultSchema("EMS"); // Default schema for employee management entities

            #endregion

            #region Seeding
            
            #endregion
        }
    }
}
