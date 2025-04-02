using RookiesWebApp.Models;

namespace RookiesWebApp.Services;

public class RookiesService(List<Person> backingStore) : IRookiesService
{
    public PaginatedList<Person> GetAllMembers(PaginationRequest paginationRequest)
    {
        return backingStore.Create(paginationRequest.PageIndex, paginationRequest.PageSize);
    }

    public Person? GetMemberById(int id)
    {
        return backingStore.FirstOrDefault(x => x.Id == id);
    }

    public void CreateMember(Person person)
    {
        var sequentialOrder = backingStore.Max(x => x.Id);
        person.Id = sequentialOrder + 1;
        person.Gender = string.Equals(person.Gender, "Male", StringComparison.OrdinalIgnoreCase)
            ? "Male"
            : "Female";
        backingStore.Add(person);
    }

    public void UpdateMemberById(int id, Person person)
    {
        var existingPerson = backingStore.First(x => x.Id == id);

        existingPerson.FirstName = person.FirstName;
        existingPerson.LastName = person.LastName;
        existingPerson.DateOfBirth = person.DateOfBirth;
        existingPerson.PhoneNumber = person.PhoneNumber;
        existingPerson.Gender = string.Equals(person.Gender, "Male", StringComparison.OrdinalIgnoreCase)
            ? "Male"
            : "Female";
        existingPerson.BirthPlace = person.BirthPlace;
        existingPerson.IsGraduated = person.IsGraduated;
    }

    public void DeleteMember(Person person)
    {
        backingStore.Remove(person);
    }

    public PaginatedList<Person> GetAllMaleMembers(PaginationRequest paginationRequest)
    {
        return backingStore
            .Where(x => x.Gender is "Male")
            .OrderBy(x => x.DateOfBirth)
            .ToList()
            .Create(paginationRequest.PageIndex, paginationRequest.PageSize);
    }

    public Person? GetOldestMember()
    {
        if (backingStore.Count == 0)
        {
            return null;
        }

        return backingStore.MaxBy(x => x.DateOfBirth);
    }

    public IReadOnlyList<string> GetAllMembersName()
    {
        return backingStore.Select(x => $"{x.FirstName} {x.LastName}")
            .ToList();
    }

    public PaginatedList<Person> GetAllMembersWithPredicate(ComparisonOperatorType specification,
        PaginationRequest paginationRequest)
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

        return members.Create(paginationRequest.PageIndex, paginationRequest.PageSize);
    }
}