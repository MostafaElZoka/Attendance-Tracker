
using Data_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace Data_Layer.Context;

public class AttendanceDbContext:DbContext
{
    internal DbSet<Employee> Employees { get; set; }
    internal DbSet<Department> Departments { get; set; }
    internal DbSet<Attendance> Attendances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>()
            .OwnsOne(e => e.FullName);

        modelBuilder.Entity<Department>()
            .HasMany(d => d.Employees);

        modelBuilder.Entity<Attendance>()
            .HasIndex(a => new { a.EmployeeId, a.Date }).IsUnique();
    }
}
