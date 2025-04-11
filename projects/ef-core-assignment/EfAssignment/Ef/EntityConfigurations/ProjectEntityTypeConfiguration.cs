using EfAssignment.Ef.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfAssignment.Ef.EntityConfigurations;

public class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();

        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#many-to-many-with-class-for-join-entity
        builder.HasMany(e => e.Employees)
            .WithMany(e => e.Projects)
            .UsingEntity<ProjectEmployee>();

        builder.HasMany<ProjectEmployee>()
            .WithOne()
            .HasForeignKey(ep => ep.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}