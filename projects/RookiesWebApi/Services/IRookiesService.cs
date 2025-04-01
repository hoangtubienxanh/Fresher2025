using RookiesWebApi.Models;

namespace RookiesWebApi.Services;

public interface IRookiesService
{
    IReadOnlyList<Person> GetAllMembers();
    IReadOnlyList<Person> GetAllMaleMembers();
    Person? GetOldestMember();
    IReadOnlyList<string> GetAllMembersName();
    IReadOnlyList<Person> GetAllMembersWithPredicate(ComparisonOperatorType specification);
}