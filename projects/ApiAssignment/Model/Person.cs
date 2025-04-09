namespace ApiAssignment.Model;

public class Person
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? BirthPlace { get; set; }
}

public enum Gender
{
    Male,
    Female,
    Other
}