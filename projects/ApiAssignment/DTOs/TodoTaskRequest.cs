namespace ApiAssignment.DTOs;

public record TodoTaskRequest
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public bool IsCompleted { get; init; }
    public int? AssigneeId { get; init; }
    public DateTime? DueDate { get; init; }
}