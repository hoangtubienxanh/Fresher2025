namespace EfAssignment.Ef.Models;

public class Project
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public List<Employee> Employees { get; } = [];
}