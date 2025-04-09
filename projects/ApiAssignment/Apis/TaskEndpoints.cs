using ApiAssignment.DTOs;
using ApiAssignment.Model;
using ApiAssignment.Services;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiAssignment.Apis;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/task").WithOpenApi();

        group.MapGet("/", GetAllTasks)
            .WithName("GetAllTasks");

        group.MapGet("/{id}", GetTaskById)
            .WithName("GetTaskById");

        group.MapPut("/{id}", UpdateTask)
            .WithName("UpdateTask");

        group.MapPost("/", CreateTask)
            .WithName("CreateTask");

        group.MapPost("/bulk", BulkCreateTasks)
            .WithName("BulkCreateTasks");

        group.MapDelete("/{id}", DeleteTask)
            .WithName("DeleteTask");

        group.MapDelete("/bulk", BulkDeleteTasks)
            .WithName("BulkDeleteTasks");
    }

    private static async Task<Results<NoContent, UnprocessableEntity>> BulkDeleteTasks(
        [FromBody] BulkTaskIdsRequest request,
        [FromServices] ITodoTaskService service)
    {
        if (request.Contains.Length == 0)
        {
            return TypedResults.UnprocessableEntity();
        }

        foreach (var id in request.Contains)
        {
            await service.DeleteTaskAsync(id);
        }

        return TypedResults.NoContent();
    }

    private static async Task<Results<NoContent, NotFound>> DeleteTask(int id, [FromServices] ITodoTaskService service)
    {
        var result = await service.DeleteTaskAsync(id);
        if (!result)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.NoContent();
    }

    private static async Task<Results<Created<List<TodoTaskResponse>>, ValidationProblem>> BulkCreateTasks(
        List<TodoTaskRequest> requests,
        [FromServices] ITodoTaskService service)
    {
        if (requests.Count == 0)
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Tasks", ["At least one task must be provided"] }
            });
        }

        var createdTasks = new List<TodoTaskResponse>();
        foreach (var taskModel in requests.Select(AsTodoTask))
        {
            var createdTask = await service.CreateTaskAsync(taskModel);
            createdTasks.Add(AsTodoTaskResponse(createdTask));
        }

        return TypedResults.Created("/api/task", createdTasks);
    }

    private static async Task<Results<Created<TodoTaskResponse>, ValidationProblem>> CreateTask(
        TodoTaskRequest request,
        [FromServices] ITodoTaskService service)
    {
        if (string.IsNullOrEmpty(request.Title))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Title", ["Title is required"] }
            });
        }

        var taskModel = AsTodoTask(request);
        var createdTask = await service.CreateTaskAsync(taskModel);
        var response = AsTodoTaskResponse(createdTask);

        return TypedResults.Created($"/api/task/{createdTask.Id}", response);
    }

    private static async Task<Results<NoContent, NotFound, ValidationProblem>> UpdateTask(
        int id,
        TodoTaskRequest request,
        [FromServices] ITodoTaskService service)
    {
        if (string.IsNullOrEmpty(request.Title))
        {
            return TypedResults.ValidationProblem(new Dictionary<string, string[]>
            {
                { "Title", ["Title is required"] }
            });
        }

        var taskModel = AsTodoTask(request);
        var updatedTask = await service.UpdateTaskAsync(id, taskModel);

        if (updatedTask == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.NoContent();
    }

    private static async Task<Results<Ok<TodoTaskResponse>, NotFound>> GetTaskById(
        int id,
        [FromServices] ITodoTaskService service)
    {
        var task = await service.GetTaskByIdAsync(id);
        if (task is null)
        {
            return TypedResults.NotFound();
        }

        var response = AsTodoTaskResponse(task);
        return TypedResults.Ok(response);
    }

    private static async Task<Ok<IEnumerable<TodoTaskResponse>>> GetAllTasks([FromServices] ITodoTaskService service)
    {
        var tasks = await service.GetAllTasksAsync();
        var responses = tasks.Select(AsTodoTaskResponse);
        return TypedResults.Ok(responses);
    }

    private static TodoTask AsTodoTask(TodoTaskRequest request)
    {
        return new TodoTask { Title = request.Title, IsCompleted = request.IsCompleted };
    }

    private static TodoTaskResponse AsTodoTaskResponse(TodoTask task)
    {
        return new TodoTaskResponse { Id = task.Id, Title = task.Title, IsCompleted = task.IsCompleted };
    }
}