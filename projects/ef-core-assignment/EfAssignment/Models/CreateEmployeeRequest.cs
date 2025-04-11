namespace EfAssignment.Models;

public record CreateEmployeeRequest(
    string Name,
    int DepartmentId,
    DateOnly JoinedDate,
    int SalaryAmount);