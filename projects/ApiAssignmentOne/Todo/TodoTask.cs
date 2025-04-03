namespace ApiAssignmentOne.Todo;

public class TodoTask
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public bool Completed { get; set; }
}