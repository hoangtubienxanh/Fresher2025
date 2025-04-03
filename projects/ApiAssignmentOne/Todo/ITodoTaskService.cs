namespace ApiAssignmentOne.Todo;

public interface ITodoTaskService
{
    List<TodoTask> GetAllTasks();

    TodoTask? GetTaskById(int id);

    TodoTask AddTask(TodoTask todoTask);

    List<TodoTask> AddTasks(IEnumerable<TodoTask> tasks);

    bool UpdateTask(int id, TodoTask updatedTodoTask);

    bool DeleteTask(int id);

    int DeleteTasks(IEnumerable<int> ids);
}