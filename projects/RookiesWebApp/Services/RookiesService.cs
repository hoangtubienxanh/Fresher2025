using System.Globalization;

using CsvHelper;

using RookiesWebApp.Models;

namespace RookiesWebApp.Services;

public class RookiesService(List<Person> backingStore) : IRookiesService
{
    public byte[] ExportMembersToExcel(PaginationRequest paginationRequest)
    {
        using var memoryStream = new MemoryStream();
        using var writer = new StreamWriter(memoryStream);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<PersonCsvMapping>();
        csv.WriteRecords(GetAllMembers(paginationRequest));
        // from docs: https://joshclose.github.io/CsvHelper/getting-started/#:~:text=After%20you%20are%20done%20writing%2C%20you%20should%20call%20writer.Flush()%20to%20ensure%20that%20all%20the%20data%20in%20the%20writer%27s%20internal%20buffer%20has%20been%20flushed%20to%20the%20file
        writer.Flush();
        return memoryStream.ToArray();
    }
    
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

        return backingStore.MinBy(x => x.DateOfBirth);
    }

    public string GetAllMembersName()
    {
        return string.Join(", ", backingStore.Select(x => $"{x.FirstName} {x.LastName}"));
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