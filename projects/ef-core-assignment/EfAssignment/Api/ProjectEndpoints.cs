using EfAssignment.Models;
using EfAssignment.Service;

using Microsoft.AspNetCore.Mvc;

namespace EfAssignment.Api;

public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/project").WithOpenApi();

        group.MapGet("/", GetAllProjects);
        group.MapGet("/{id}", GetProjectById);
        group.MapPost("/", CreateProject);
        group.MapPut("/{id}", UpdateProject);
        group.MapDelete("/{id}", DeleteProject);
        group.MapPost("/{id}/enroll", EnrollEmployee);
    }

    private static async Task<IResult> GetAllProjects([FromServices] IProjectService service)
    {
        return TypedResults.Ok(await service.GetAsync());
    }

    private static async Task<IResult> GetProjectById(int id, [FromServices] IProjectService service)
    {
        var project = await service.GetByIdAsync(id);
        return project is null ? TypedResults.NotFound() : TypedResults.Ok(project);
    }

    private static async Task<IResult> CreateProject(
        CreateProjectRequest request,
        [FromServices] IProjectService service)
    {
        var project = await service.CreateAsync(request);
        return TypedResults.Created($"/api/project/{project.Id}", project);
    }

    private static async Task<IResult> UpdateProject(
        int id,
        UpdateProjectRequest request,
        [FromServices] IProjectService service)
    {
        var project = await service.UpdateAsync(id, request);
        return project is null ? TypedResults.NotFound() : TypedResults.Ok(project);
    }

    private static async Task<IResult> DeleteProject(int id, [FromServices] IProjectService service)
    {
        var result = await service.DeleteAsync(id);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }

    private static async Task<IResult> EnrollEmployee(
        int id,
        EnrollEmployeeRequest request,
        [FromServices] IProjectService service)
    {
        var result = await service.EnrollEmployeeAsync(id, request);
        return result ? TypedResults.Ok() : TypedResults.UnprocessableEntity();
    }
}