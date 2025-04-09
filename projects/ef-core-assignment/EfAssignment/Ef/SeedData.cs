using EfAssignment.Ef.Models;

using Microsoft.EntityFrameworkCore;

namespace EfAssignment.Ef;

public static class SeedData
{
    public static void Seed(DbContext context, bool migrated)
    {
        if (context is not OneDbContext oneDbContext)
        {
            return;
        }

        if (!oneDbContext.Departments.Any())
        {
            oneDbContext.Departments.AddRange(
                new Department { Name = "Software Development" },
                new Department { Name = "Finance" },
                new Department { Name = "Accountant" },
                new Department { Name = "HR" }
            );
        }

        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, bool migrated, CancellationToken cancellationToken)
    {
        if (context is not OneDbContext oneDbContext)
        {
            return;
        }

        if (!await oneDbContext.Departments.AnyAsync(cancellationToken))
        {
            await oneDbContext.Departments.AddRangeAsync(
                new Department { Name = "Software Development" },
                new Department { Name = "Finance" },
                new Department { Name = "Accountant" },
                new Department { Name = "HR" }
            );
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}