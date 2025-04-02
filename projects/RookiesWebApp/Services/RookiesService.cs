using RookiesWebApp.Models;

namespace RookiesWebApp.Services;

public class RookiesService(List<Person> backingStore) : IRookiesService
{
    public IReadOnlyList<Person> GetAllMembers()
    {
        return backingStore;
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

    public Person? GetMemberById(int id)
    {
        return backingStore.FirstOrDefault(x => x.Id == id);
    }

    public void UpdateMemberById(int id, Person personModel)
    {
        var existingPerson = backingStore.First(x => x.Id == id);

        existingPerson.FirstName = personModel.FirstName;
        existingPerson.LastName = personModel.LastName;
        existingPerson.DateOfBirth = personModel.DateOfBirth;
        existingPerson.PhoneNumber = personModel.PhoneNumber;
        existingPerson.Gender = string.Equals(personModel.Gender, "Male", StringComparison.OrdinalIgnoreCase)
            ? "Male"
            : "Female";
        existingPerson.BirthPlace = personModel.BirthPlace;
        existingPerson.IsGraduated = personModel.IsGraduated;
    }

    public void RemoveMember(Person person)
    {
        backingStore.Remove(person);
    }

    public IReadOnlyList<Person> GetAllMaleMembers()
    {
        return backingStore.Where(x => x.Gender is "Male").OrderBy(x => x.DateOfBirth).ToList();
    }

    public Person? GetOldestMember()
    {
        if (backingStore.Count == 0)
            return null;

        return backingStore
            .Select((item, index) => (index, item))
            .OrderBy(x => x.index)
            .MaxBy(x => x.item.DateOfBirth)
            .item;
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