namespace ApiAssignment.DTOs;

public record BulkTaskIdsRequest
{
    public required int[] Contains { get; init; }
}