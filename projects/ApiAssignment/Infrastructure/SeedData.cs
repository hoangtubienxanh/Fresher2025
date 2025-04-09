using ApiAssignment.Model;

using Microsoft.EntityFrameworkCore;

namespace ApiAssignment.Infrastructure;

public static class SeedData
{
    private static readonly List<TodoTask> SampleTasks =
    [
        new() { Id = 1, Title = "Complete project documentation", IsCompleted = false },
        new() { Id = 2, Title = "Fix bugs in authentication module", IsCompleted = true },
        new() { Id = 3, Title = "Implement new feature request", IsCompleted = false },
        new() { Id = 4, Title = "Code review for PR #42", IsCompleted = false },
        new() { Id = 5, Title = "Update dependencies", IsCompleted = false }
    ];

    private static readonly List<Person> SamplePersons =
    [
        new()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            DateOfBirth = new DateOnly(1994, 5, 15),
            Gender = Gender.Male,
            BirthPlace = "New York"
        },
        new()
        {
            Id = 2,
            FirstName = "Jane",
            LastName = "Smith",
            DateOfBirth = new DateOnly(1999, 8, 22),
            Gender = Gender.Female,
            BirthPlace = "Los Angeles"
        },
        new()
        {
            Id = 3,
            FirstName = "Michael",
            LastName = "Johnson",
            DateOfBirth = new DateOnly(1989, 3, 10),
            Gender = Gender.Male,
            BirthPlace = "Chicago"
        },
        new()
        {
            Id = 4,
            FirstName = "Emily",
            LastName = "Davis",
            DateOfBirth = new DateOnly(1996, 11, 5),
            Gender = Gender.Female,
            BirthPlace = "Boston"
        },
        new()
        {
            Id = 5,
            FirstName = "Robert",
            LastName = "Wilson",
            DateOfBirth = new DateOnly(1984, 7, 18),
            Gender = Gender.Male,
            BirthPlace = "Miami"
        }
    ];

    public static void Seed(DbContext context, bool migrated)
    {
        var todoTaskDbSet = context.Set<TodoTask>();
        if (!todoTaskDbSet.Any())
        {
            todoTaskDbSet.AddRange(SampleTasks);
        }

        var personDbSet = context.Set<Person>();
        if (!personDbSet.Any())
        {
            personDbSet.AddRange(SamplePersons);
        }

        context.SaveChanges();
    }

    public static async Task SeedAsync(DbContext context, bool migrated, CancellationToken arg3)
    {
        if (!migrated)
        {
            return;
        }

        var todoTaskDbSet = context.Set<TodoTask>();
        if (!todoTaskDbSet.Any())
        {
            todoTaskDbSet.AddRange(SampleTasks);
        }

        var personDbSet = context.Set<Person>();
        if (!personDbSet.Any())
        {
            personDbSet.AddRange(SamplePersons);
        }

        await context.SaveChangesAsync(arg3);
    }
}