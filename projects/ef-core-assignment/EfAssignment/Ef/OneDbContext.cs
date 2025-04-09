using EfAssignment.Ef.EntityConfigurations;
using EfAssignment.Ef.Models;

using Microsoft.EntityFrameworkCore;

namespace EfAssignment.Ef;

public class OneDbContext(DbContextOptions<OneDbContext> options) : DbContext(options)
{
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Project> Projects { get; set; }

    public DbSet<Salaries> Salaries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DepartmentEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new EmployeeEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ProjectEntityTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}