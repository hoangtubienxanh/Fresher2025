using EfAssignment.Ef.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfAssignment.Ef.EntityConfigurations;

public class SalariesEntityTypeConfiguration : IEntityTypeConfiguration<Salaries>
{
    public void Configure(EntityTypeBuilder<Salaries> builder)
    {
        builder.HasOne<Employee>()
            .WithOne(e => e.Salary)
            .HasForeignKey<Salaries>(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}