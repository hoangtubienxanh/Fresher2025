using ApiAssignment.Infrastructure.Repositories;
using ApiAssignment.Model;

namespace ApiAssignment.Services;

public class TodoTaskService(ITodoTaskRepository repository) : ITodoTaskService
{
    public async Task<IEnumerable<TodoTask>> GetAllTasksAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<TodoTask?> GetTaskByIdAsync(int id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task<TodoTask> CreateTaskAsync(TodoTask task)
    {
        task.IsCompleted = false;
        return await repository.CreateAsync(task);
    }

    public async Task<TodoTask?> UpdateTaskAsync(int id, TodoTask task)
    {
        var existingTask = await repository.GetByIdAsync(id);
        if (existingTask == null)
        {
            return null;
        }

        existingTask.Title = task.Title;
        existingTask.IsCompleted = task.IsCompleted;

        return await repository.UpdateAsync(existingTask);
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        return await repository.DeleteAsync(id);
    }
}