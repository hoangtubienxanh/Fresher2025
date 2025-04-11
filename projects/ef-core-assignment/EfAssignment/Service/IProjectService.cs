using EfAssignment.Ef.Models;
using EfAssignment.Models;

namespace EfAssignment.Service;

public interface IProjectService
{
    Task<List<Project>> GetAsync();
    Task<Project?> GetByIdAsync(int id);
    Task<Project> CreateAsync(CreateProjectRequest request);
    Task<Project?> UpdateAsync(int id, UpdateProjectRequest request);
    Task<bool> DeleteAsync(int id);
    Task<bool> EnrollEmployeeAsync(int projectId, EnrollEmployeeRequest request);
}