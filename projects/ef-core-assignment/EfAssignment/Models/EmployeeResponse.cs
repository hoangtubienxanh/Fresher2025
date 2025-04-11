namespace EfAssignment.Models;

public record EmployeeResponse(
    int Id,
    string FullName,
    int SalaryAmount,
    DateOnly JoinedDate);