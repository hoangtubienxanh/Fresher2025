using ApiAssignment.Model;

namespace ApiAssignment.DTOs;

public record CreatePersonRequest
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public Gender Gender { get; init; }
    public DateOnly DateOfBirth { get; init; }
    public required string BirthPlace { get; init; }
}