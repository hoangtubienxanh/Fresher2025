using ApiAssignment.Infrastructure.Repositories;
using ApiAssignment.Model;

namespace ApiAssignment.Services;

public class PersonService(IPersonRepository repository) : IPersonService
{
    public async Task<List<Person>> GetAllPersonsAsync()
    {
        var persons = await repository.GetAllAsync();
        return persons.ToList();
    }

    public async Task<Person?> GetPersonByIdAsync(int id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task<Person> AddPersonAsync(Person person)
    {
        return await repository.CreateAsync(person);
    }

    public async Task<Person?> UpdatePersonAsync(int id, Person person)
    {
        person.Id = id;
        return await repository.UpdateAsync(person);
    }

    public async Task<bool> DeletePersonAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }

    public async Task<List<Person>> FilterByNameAsync(string firstName, string lastName)
    {
        var persons = await repository.FilterByNameAsync(firstName, lastName);
        return persons.ToList();
    }

    public async Task<List<Person>> FilterByGenderAsync(Gender gender)
    {
        var persons = await repository.FilterByGenderAsync(gender);
        return persons.ToList();
    }

    public async Task<List<Person>> FilterByBirthPlaceAsync(string birthPlace)
    {
        var persons = await repository.FilterByBirthPlaceAsync(birthPlace);
        return persons.ToList();
    }
}