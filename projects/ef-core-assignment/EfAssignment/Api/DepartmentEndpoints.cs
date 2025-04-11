using EfAssignment.Models;
using EfAssignment.Service;

using Microsoft.AspNetCore.Mvc;

namespace EfAssignment.Api;

public static class DepartmentEndpoints
{
    public static void MapDepartmentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/department").WithOpenApi();

        group.MapGet("/", GetAllDepartments);
        group.MapGet("/{id}", GetDepartmentById);
        group.MapPost("/", CreateDepartment);
        group.MapPut("/{id}", UpdateDepartment);
        group.MapDelete("/{id}", DeleteDepartment);
    }

    private static async Task<IResult> GetAllDepartments([FromServices] IDepartmentService service)
    {
        return TypedResults.Ok(await service.GetAsync());
    }

    private static async Task<IResult> GetDepartmentById(int id, [FromServices] IDepartmentService service)
    {
        var department = await service.GetByIdAsync(id);
        return department is null ? TypedResults.NotFound() : TypedResults.Ok(department);
    }

    private static async Task<IResult> CreateDepartment(
        CreateDepartmentRequest request,
        [FromServices] IDepartmentService service)
    {
        var department = await service.CreateAsync(request);
        return TypedResults.Created($"/api/department/{department.Id}", department);
    }

    private static async Task<IResult> UpdateDepartment(
        int id,
        UpdateDepartmentRequest request,
        [FromServices] IDepartmentService service)
    {
        var department = await service.UpdateAsync(id, request);
        return department is null ? TypedResults.NotFound() : TypedResults.Ok(department);
    }

    private static async Task<IResult> DeleteDepartment(int id, [FromServices] IDepartmentService service)
    {
        var result = await service.DeleteAsync(id);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}