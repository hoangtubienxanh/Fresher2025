using ApiAssignment.DTOs;
using ApiAssignment.Model;
using ApiAssignment.Services;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiAssignment.Apis;

public static class PersonEndpoints
{
    public static void MapPersonEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/person").WithTags("Person");

        group.MapGet("/", GetAllPersons);
        group.MapGet("/{id}", GetPersonById);
        group.MapPost("/", CreatePerson);
        group.MapPut("/{id}", UpdatePerson);
        group.MapDelete("/{id}", DeletePerson);

        group.MapGet("/filter/name", FilterByName);
        group.MapGet("/filter/gender/{gender}", FilterByGender);
        group.MapGet("/filter/birthplace/{birthPlace}", FilterByBirthPlace);
    }

    private static async Task<Ok<List<PersonResponse>>> GetAllPersons([FromServices] IPersonService service)
    {
        var persons = await service.GetAllPersonsAsync();
        var response = persons.Select(AsPersonCreatedResponse).ToList();
        return TypedResults.Ok(response);
    }

    private static async Task<Results<Ok<PersonResponse>, NotFound>> GetPersonById(int id,
        [FromServices] IPersonService service)
    {
        var person = await service.GetPersonByIdAsync(id);
        if (person == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(AsPersonCreatedResponse(person));
    }

    private static async Task<Results<Created<PersonResponse>, ValidationProblem>> CreatePerson(
        [FromBody] CreatePersonRequest request, [FromServices] IPersonService service)
    {
        if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Name", ["First name and last name are required"] }
            });
        }

        var person = new Person
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth,
            BirthPlace = request.BirthPlace
        };

        var createdPerson = await service.AddPersonAsync(person);
        return TypedResults.Created($"/api/person/{createdPerson.Id}", AsPersonCreatedResponse(createdPerson));
    }

    private static async Task<Results<NoContent, NotFound, ValidationProblem>> UpdatePerson(
        int id, [FromBody] CreatePersonRequest request, [FromServices] IPersonService service)
    {
        if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Name", ["First name and last name are required"] }
            });
        }

        var person = new Person
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth,
            BirthPlace = request.BirthPlace
        };

        var result = await service.UpdatePersonAsync(id, person);
        if (result == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.NoContent();
    }

    private static async Task<Results<NoContent, NotFound>> DeletePerson(int id, [FromServices] IPersonService service)
    {
        var result = await service.DeletePersonAsync(id);
        if (!result)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.NoContent();
    }

    private static async Task<Results<Ok<List<PersonResponse>>, BadRequest>> FilterByName(
        string? firstName, string? lastName, [FromServices] IPersonService service)
    {
        if (string.IsNullOrWhiteSpace(firstName) && string.IsNullOrWhiteSpace(lastName))
        {
            return TypedResults.BadRequest();
        }

        var persons = await service.FilterByNameAsync(firstName!, lastName!);
        var response = persons.Select(AsPersonCreatedResponse).ToList();
        return TypedResults.Ok(response);
    }

    private static async Task<Ok<List<PersonResponse>>> FilterByGender(Gender gender,
        [FromServices] IPersonService service)
    {
        var persons = await service.FilterByGenderAsync(gender);
        var response = persons.Select(AsPersonCreatedResponse).ToList();
        return TypedResults.Ok(response);
    }

    private static async Task<Ok<List<PersonResponse>>> FilterByBirthPlace(string birthPlace,
        [FromServices] IPersonService service)
    {
        var persons = await service.FilterByBirthPlaceAsync(birthPlace);
        var response = persons.Select(AsPersonCreatedResponse).ToList();
        return TypedResults.Ok(response);
    }


    private static PersonResponse AsPersonCreatedResponse(Person person)
    {
        return new PersonResponse
        {
            Id = person.Id, FirstName = person.FirstName, LastName = person.LastName, BirthPlace = person.BirthPlace
        };
    }
}