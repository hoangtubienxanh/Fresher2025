namespace ApiAssignment.DTOs;

public record PersonResponse
{
    public int Id { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? BirthPlace { get; init; }
}