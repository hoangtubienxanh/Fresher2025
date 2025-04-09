using ApiAssignment.Model;

namespace ApiAssignment.Services;

public interface IPersonService
{
    Task<List<Person>> GetAllPersonsAsync();
    Task<Person?> GetPersonByIdAsync(int id);
    Task<Person> AddPersonAsync(Person person);
    Task<Person?> UpdatePersonAsync(int id, Person person);
    Task<bool> DeletePersonAsync(int id);
    Task<List<Person>> FilterByNameAsync(string firstName, string lastName);
    Task<List<Person>> FilterByGenderAsync(Gender gender);
    Task<List<Person>> FilterByBirthPlaceAsync(string birthPlace);
}