using ApiAssignment.Model;

using Microsoft.EntityFrameworkCore;

namespace ApiAssignment.Infrastructure.Repositories;

public class PersonRepository(TodoTaskDbContext context) : IPersonRepository
{
    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await context.Persons.ToListAsync();
    }

    public async Task<Person?> GetByIdAsync(int id)
    {
        return await context.Persons.FindAsync(id);
    }

    public async Task<Person> CreateAsync(Person person)
    {
        context.Persons.Add(person);
        await context.SaveChangesAsync();
        return person;
    }

    public async Task<Person?> UpdateAsync(Person person)
    {
        context.Update(person);
        await context.SaveChangesAsync();
        return person;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var person = await context.Persons.FindAsync(id);
        if (person == null)
        {
            return false;
        }

        context.Persons.Remove(person);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Person>> FilterByNameAsync(string firstName, string lastName)
    {
        return await context.Persons
            .Where(p => p.FirstName == firstName && p.LastName == lastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Person>> FilterByGenderAsync(Gender gender)
    {
        return await context.Persons
            .Where(p => p.Gender == gender)
            .ToListAsync();
    }

    public async Task<IEnumerable<Person>> FilterByBirthPlaceAsync(string birthPlace)
    {
        return await context.Persons
            .Where(p => p.BirthPlace == birthPlace)
            .ToListAsync();
    }
}