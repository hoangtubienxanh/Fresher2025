using ApiAssignment.Model;

namespace ApiAssignment.Infrastructure.Repositories;

public interface IPersonRepository
{
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person?> GetByIdAsync(int id);
    Task<Person> CreateAsync(Person person);
    Task<Person?> UpdateAsync(Person person);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<Person>> FilterByNameAsync(string firstName, string lastName);
    Task<IEnumerable<Person>> FilterByGenderAsync(Gender gender);
    Task<IEnumerable<Person>> FilterByBirthPlaceAsync(string birthPlace);
}