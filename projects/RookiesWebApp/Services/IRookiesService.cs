using RookiesWebApp.Models;

namespace RookiesWebApp.Services;

public interface IRookiesService
{
    IReadOnlyList<Person> GetAllMembers();
    void CreateMember(Person person);
    Person? GetMemberById(int id);
    void UpdateMemberById(int id, Person personModel);
    void RemoveMember(Person person);
    IReadOnlyList<Person> GetAllMaleMembers();
    Person? GetOldestMember();
    IReadOnlyList<string> GetAllMembersName();
    IReadOnlyList<Person> GetAllMembersWithPredicate(ComparisonOperatorType specification);
}