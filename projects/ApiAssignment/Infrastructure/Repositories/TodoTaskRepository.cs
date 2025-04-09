using ApiAssignment.Model;

using Microsoft.EntityFrameworkCore;

namespace ApiAssignment.Infrastructure.Repositories;

public class TodoTaskRepository(TodoTaskDbContext context) : ITodoTaskRepository
{
    public async Task<IEnumerable<TodoTask>> GetAllAsync()
    {
        return await context.Tasks.ToListAsync();
    }

    public async Task<TodoTask?> GetByIdAsync(int id)
    {
        return await context.Tasks.FindAsync(id);
    }

    public async Task<TodoTask> CreateAsync(TodoTask task)
    {
        context.Tasks.Add(task);
        await context.SaveChangesAsync();
        return task;
    }

    public async Task<TodoTask?> UpdateAsync(TodoTask? task)
    {
        context.Entry(task).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await context.Tasks.FindAsync(id);
        if (task == null)
        {
            return false;
        }

        context.Tasks.Remove(task);
        await context.SaveChangesAsync();
        return true;
    }
}