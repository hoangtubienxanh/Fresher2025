using ApiAssignment.Model;

using Microsoft.EntityFrameworkCore;

namespace ApiAssignment.Infrastructure;

public class TodoTaskDbContext(DbContextOptions<TodoTaskDbContext> options) : DbContext(options)
{
    public DbSet<TodoTask> Tasks => Set<TodoTask>();
    public DbSet<Person> Persons => Set<Person>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>()
            .Property(p => p.Gender)
            .HasConversion<string>();
    }
}