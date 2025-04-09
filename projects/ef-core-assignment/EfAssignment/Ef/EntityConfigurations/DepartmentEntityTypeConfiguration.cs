using EfAssignment.Ef.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfAssignment.Ef.EntityConfigurations;

public class DepartmentEntityTypeConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();

        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/one-to-many#one-to-many-without-navigation-to-principal
        builder.HasMany(e => e.Employees)
            .WithOne()
            .HasForeignKey(e => e.DepartmentId)
            .IsRequired();
    }
}