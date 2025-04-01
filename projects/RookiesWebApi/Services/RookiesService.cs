using RookiesWebApi.Models;

namespace RookiesWebApi.Services;

public class RookiesService(List<Person> backingStore) : IRookiesService
{
    public IReadOnlyList<Person> GetAllMembers()
    {
        return backingStore;
    }

    public IReadOnlyList<Person> GetAllMaleMembers()
    {
        return backingStore.Where(x => x.Gender is "Male").OrderBy(x => x.DateOfBirth).ToList();
    }

    public Person? GetOldestMember()
    {
        // Makes sense to sort first, see link:
        // https://github.com/dotnet/runtime/blob/bbc50e511150368fd10f56783c48aa3a04f1f5d3/src/libraries/System.Linq/src/System/Linq/Max.cs#L476
        return backingStore.OrderBy(x => x.DateOfBirth).MaxBy(x => x.DateOfBirth);
    }

    public IReadOnlyList<string> GetAllMembersName()
    {
        return backingStore.Select(x => $"{x.FirstName} {x.LastName}").ToList();
    }

    public IReadOnlyList<Person> GetAllMembersWithPredicate(ComparisonOperatorType specification)
    {
        var query = backingStore.OrderBy(x => x.DateOfBirth);
        List<Person> members = specification switch
        {
            ComparisonOperatorType.Equals => query
                .Where(x => x.DateOfBirth.Year == 2000)
                .ToList(),
            ComparisonOperatorType.GreaterThan => query
                .Where(x => x.DateOfBirth.Year > 2000)
                .ToList(),
            ComparisonOperatorType.LessThan => query
                .Where(x => x.DateOfBirth.Year < 2000)
                .ToList(),
            _ => []
        };

        return members;
    }
}