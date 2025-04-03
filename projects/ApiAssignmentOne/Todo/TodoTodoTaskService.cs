namespace ApiAssignmentOne.Todo;

public class TodoTodoTaskService : ITodoTaskService
{
    private readonly List<TodoTask> _tasks = new();
    private int _nextId = 1;

    public List<TodoTask> GetAllTasks() => _tasks;

    public TodoTask? GetTaskById(int id) => _tasks.FirstOrDefault(t => t.Id == id);

    public TodoTask AddTask(TodoTask task)
    {
        task.Id = _nextId++;
        _tasks.Add(task);
        return task;
    }

    public List<TodoTask> AddTasks(IEnumerable<TodoTask> tasks)
    {
        var addedTasks = new List<TodoTask>();
        foreach (var task in tasks)
        {
            task.Id = _nextId++;
            _tasks.Add(task);
            addedTasks.Add(task);
        }

        return addedTasks;
    }

    public bool UpdateTask(int id, TodoTask updatedTask)
    {
        var existingTask = _tasks.FirstOrDefault(t => t.Id == id);
        if (existingTask == null) return false;

        existingTask.Title = updatedTask.Title;
        existingTask.Completed = updatedTask.Completed;
        return true;
    }

    public bool DeleteTask(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task == null) return false;

        return _tasks.Remove(task);
    }

    public int DeleteTasks(IEnumerable<int> ids)
    {
        int count = 0;
        foreach (var id in ids)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null && _tasks.Remove(task))
            {
                count++;
            }
        }

        return count;
    }
}