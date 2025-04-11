namespace EfAssignment.Models;

public record UpdateEmployeeRequest(
    string Name,
    int DepartmentId,
    DateOnly JoinedDate,
    int SalaryAmount);