using ApiAssignment.Model;

namespace ApiAssignment.Infrastructure.Repositories;

public interface ITodoTaskRepository
{
    Task<IEnumerable<TodoTask>> GetAllAsync();
    Task<TodoTask?> GetByIdAsync(int id);
    Task<TodoTask> CreateAsync(TodoTask task);
    Task<TodoTask?> UpdateAsync(TodoTask? task);
    Task<bool> DeleteAsync(int id);
}