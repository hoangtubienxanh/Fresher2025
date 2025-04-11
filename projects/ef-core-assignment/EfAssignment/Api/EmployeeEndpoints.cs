using EfAssignment.Models;
using EfAssignment.Service;

using Microsoft.AspNetCore.Mvc;

namespace EfAssignment.Api;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/employee").WithOpenApi();

        group.MapGet("/with-departments", GetEmployeesWithDepartments);
        group.MapGet("/with-projects", GetEmployeesWithProjects);
        group.MapGet("/filtered", GetHighPaidRecentEmployees);

        group.MapGet("/", GetAllEmployees);
        group.MapGet("/{id}", GetEmployeeById);
        group.MapPost("/", CreateEmployee);
        group.MapPut("/{id}", UpdateEmployee);
        group.MapDelete("/{id}", DeleteEmployee);
    }

    private static async Task<IResult> GetHighPaidRecentEmployees([FromServices] IEmployeeService service)
    {
        return TypedResults.Ok(await service.GetHighPaidRecentEmployeesAsync());
    }

    private static async Task<IResult> GetEmployeesWithProjects([FromServices] IEmployeeService service)
    {
        return TypedResults.Ok(await service.GetEmployeesWithProjectsAsync());
    }

    private static async Task<IResult> GetEmployeesWithDepartments([FromServices] IEmployeeService service)
    {
        return TypedResults.Ok(await service.GetEmployeesWithDepartmentsAsync());
    }

    private static async Task<IResult> GetAllEmployees([FromServices] IEmployeeService service)
    {
        return TypedResults.Ok(await service.GetAsync());
    }

    private static async Task<IResult> GetEmployeeById(int id, [FromServices] IEmployeeService service)
    {
        var employee = await service.GetByIdAsync(id);
        return employee is null ? TypedResults.NotFound() : TypedResults.Ok(employee);
    }

    private static async Task<IResult> CreateEmployee(
        CreateEmployeeRequest request,
        [FromServices] IEmployeeService service)
    {
        if (request.SalaryAmount <= 0)
        {
            return TypedResults.BadRequest("Salary must be greater than 0");
        }

        var employee = await service.CreateAsync(request);
        return TypedResults.Created($"/api/employee/{employee.Id}", employee);
    }

    private static async Task<IResult> UpdateEmployee(
        int id,
        UpdateEmployeeRequest request,
        [FromServices] IEmployeeService service)
    {
        if (request.SalaryAmount <= 0)
        {
            return TypedResults.BadRequest("Salary must be greater than 0");
        }

        var employee = await service.UpdateAsync(id, request);
        return employee is null ? TypedResults.NotFound() : TypedResults.Ok(employee);
    }

    private static async Task<IResult> DeleteEmployee(int id, [FromServices] IEmployeeService service)
    {
        var result = await service.DeleteAsync(id);
        return result ? TypedResults.NoContent() : TypedResults.NotFound();
    }
}