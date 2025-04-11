using EfAssignment.Models;

namespace EfAssignment.Service;

public interface IEmployeeService
{
    Task<List<EmployeesWithDepartments>> GetEmployeesWithDepartmentsAsync();
    Task<List<EmployeesWithProjects>> GetEmployeesWithProjectsAsync();
    Task<List<EmployeeResponse>> GetHighPaidRecentEmployeesAsync();
    Task<List<EmployeeResponse>> GetAsync();
    Task<EmployeeResponse?> GetByIdAsync(int id);
    Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request);
    Task<EmployeeResponse?> UpdateAsync(int id, UpdateEmployeeRequest request);
    Task<bool> DeleteAsync(int id);
}