using EfAssignment.Ef.Models;
using EfAssignment.Models;

namespace EfAssignment.Service;

public interface IDepartmentService
{
    Task<List<Department>> GetAsync();
    Task<Department?> GetByIdAsync(int id);
    Task<Department> CreateAsync(CreateDepartmentRequest request);
    Task<Department?> UpdateAsync(int id, UpdateDepartmentRequest request);
    Task<bool> DeleteAsync(int id);
}