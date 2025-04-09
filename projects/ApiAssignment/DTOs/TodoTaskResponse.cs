namespace ApiAssignment.DTOs;

public record TodoTaskResponse
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public bool IsCompleted { get; init; }
    public int? AssigneeId { get; init; }
    public string? AssigneeName { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? DueDate { get; init; }
}