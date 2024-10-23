using EMS.Domain.Models.EM;
using Microsoft.EntityFrameworkCore;

namespace EMS.Infrastructure.Contexts
{
    public class EMSDbContext : DbContext
    {
        public EMSDbContext(DbContextOptions<EMSDbContext> options)
            : base(options)
        {
        }

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

            // Department and Employee (1-n)
            builder.Entity<Department>()
                .HasMany(d => d.Employees)
                .WithOne(e => e.Department)
                .HasForeignKey(e => e.DepartmentId);

            // User and Employee (1-1)
            builder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.Id); // Assuming Employee.Id matches User.Id

            // Additional Employee configurations...
            builder.Entity<Employee>().HasKey(e => e.Id);
            builder.Entity<Salary>().HasKey(s => s.Id);
            builder.Entity<SalaryHistory>().HasKey(sh => sh.Id);
            builder.Entity<TimeCard>().HasKey(tc => tc.Id);
            builder.Entity<Department>().HasKey(d => d.Id);

            builder.HasDefaultSchema("EMS"); // Default schema for employee management entities
        }
    }
}
