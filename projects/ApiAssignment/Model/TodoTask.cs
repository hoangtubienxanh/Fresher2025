namespace ApiAssignment.Model;

public class TodoTask
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public bool IsCompleted { get; set; }
}