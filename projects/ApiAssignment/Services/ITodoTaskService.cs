using ApiAssignment.Model;

namespace ApiAssignment.Services;

public interface ITodoTaskService
{
    Task<IEnumerable<TodoTask>> GetAllTasksAsync();
    Task<TodoTask?> GetTaskByIdAsync(int id);
    Task<TodoTask> CreateTaskAsync(TodoTask task);
    Task<TodoTask?> UpdateTaskAsync(int id, TodoTask task);
    Task<bool> DeleteTaskAsync(int id);
}