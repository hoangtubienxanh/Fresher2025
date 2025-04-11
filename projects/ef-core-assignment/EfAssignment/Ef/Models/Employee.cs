namespace EfAssignment.Ef.Models;

public class Employee
{
    private Salaries? _salary;
    public int Id { get; set; }
    public required string Name { get; set; }

    public int DepartmentId { get; set; }

    public List<Project> Projects { get; } = [];

    public Salaries Salary
    {
        get => _salary ?? throw new InvalidOperationException("Uninitialized property: " + nameof(Salary));
        set => _salary = value;
    }

    public DateOnly JoinedDate { get; set; }
}