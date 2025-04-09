using EfAssignment.Ef.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfAssignment.Ef.EntityConfigurations;

public class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(250)
            .IsRequired();

        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-one#one-to-one-without-navigation-to-principal
        builder.HasOne(e => e.Salary)
            .WithOne()
            .HasForeignKey<Salaries>(e => e.EmployeeId)
            .IsRequired();
    }
}