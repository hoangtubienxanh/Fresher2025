namespace RookiesWebApp.Models;

public class Person
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Gender { get; set; }
    public required DateTime DateOfBirth { get; set; }
    public required string PhoneNumber { get; set; }
    public required string BirthPlace { get; set; }
    public required bool IsGraduated { get; set; }
}