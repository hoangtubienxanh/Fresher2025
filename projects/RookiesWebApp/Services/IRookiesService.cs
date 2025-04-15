using RookiesWebApp.Models;

namespace RookiesWebApp.Services;

public interface IRookiesService
{
    byte[] ExportMembersToExcel(PaginationRequest paginationRequest);
    PaginatedList<Person> GetAllMembers(PaginationRequest paginationRequest);
    void CreateMember(Person person);
    Person? GetMemberById(int id);
    void UpdateMemberById(int id, Person person);
    void DeleteMember(Person person);
    PaginatedList<Person> GetAllMaleMembers(PaginationRequest paginationRequest);
    Person? GetOldestMember();
    string GetAllMembersName();
    PaginatedList<Person> GetAllMembersWithPredicate(ComparisonOperatorType specification,
        PaginationRequest paginationRequest);
}