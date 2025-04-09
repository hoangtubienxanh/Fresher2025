namespace EfAssignment.Ef.Models;

public class Employee
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public int DepartmentId { get; set; }

    public List<Project> Projects { get; } = [];
    public Salaries? Salary { get; set; }

    public DateOnly JoinedDate { get; set; }
}