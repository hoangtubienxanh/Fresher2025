using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiAssignmentOne.Todo;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/TodoTask").WithOpenApi();

        group.MapGet("/", (ITodoTaskService service) =>
            {
                return TypedResults.Ok(service.GetAllTasks());
            })
            .WithName("GetAllTasks");

        group.MapGet("/{id}", Results<Ok<TodoTask>, NotFound> (int id, ITodoTaskService service) =>
            {
                var task = service.GetTaskById(id);
                if (task is null)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(task);
            })
            .WithName("GetTaskById");

        group.MapPut("/{id}", Results<NoContent, NotFound> (int id, TodoTask input, ITodoTaskService service) =>
            {
                if (service.UpdateTask(id, input))
                {
                    return TypedResults.NoContent();
                }

                return TypedResults.NotFound();
            })
            .WithName("UpdateTask");

        group.MapPost("/", (TodoTask model, ITodoTaskService service) =>
            {
                var createdTask = service.AddTask(model);
                return TypedResults.Created($"/api/TodoTask/{createdTask.Id}", createdTask);
            })
            .WithName("CreateTask");

        group.MapPost("/bulk", (List<TodoTask> tasks, ITodoTaskService service) =>
            {
                var createdTasks = service.AddTasks(tasks);
                return TypedResults.Created("/api/TodoTask", createdTasks);
            })
            .WithName("BulkCreateTasks");

        group.MapDelete("/{id}", Results<NoContent, NotFound> (int id, ITodoTaskService service) =>
            {
                if (service.DeleteTask(id))
                {
                    return TypedResults.NoContent();
                }

                return TypedResults.NotFound();
            })
            .WithName("DeleteTask");

        group.MapDelete("/bulk", (int[] ids, ITodoTaskService service) =>
            {
                var deletedCount = service.DeleteTasks(ids);
                return TypedResults.Ok(new { deletedCount });
            })
            .WithName("BulkDeleteTasks");
    }
}