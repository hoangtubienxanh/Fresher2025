using RookiesWebApp.Models;

namespace RookiesWebApp.Services;

public interface IRookiesService
{
    PaginatedList<Person> GetAllMembers(PaginationRequest paginationRequest);
    void CreateMember(Person person);
    Person? GetMemberById(int id);
    void UpdateMemberById(int id, Person person);
    void DeleteMember(Person person);
    PaginatedList<Person> GetAllMaleMembers(PaginationRequest paginationRequest);
    Person? GetOldestMember();
    IReadOnlyList<string> GetAllMembersName();

    PaginatedList<Person> GetAllMembersWithPredicate(ComparisonOperatorType specification,
        PaginationRequest paginationRequest);
}