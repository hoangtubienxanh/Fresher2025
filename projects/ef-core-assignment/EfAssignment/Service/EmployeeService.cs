using EfAssignment.Ef;
using EfAssignment.Ef.Models;
using EfAssignment.Models;

using Microsoft.EntityFrameworkCore;

namespace EfAssignment.Service;

public class EmployeeService(OneDbContext context) : IEmployeeService
{
    public async Task<List<EmployeesWithDepartments>> GetEmployeesWithDepartmentsAsync()
    {
        return await GetEmployeesWithDepartmentsSqlAsync();
    }


    public async Task<List<EmployeesWithProjects>> GetEmployeesWithProjectsAsync()
    {
        return await GetEmployeesWithProjectsSqlAsync();
    }

    public async Task<List<EmployeeResponse>> GetHighPaidRecentEmployeesAsync()
    {
        return await GetHighPaidRecentEmployeesSqlAsync();
    }

    public async Task<List<EmployeeResponse>> GetAsync()
    {
        return await context.Employees
            .Select(e => new EmployeeResponse(e.Id, e.Name, e.Salary.Salary, e.JoinedDate))
            .ToListAsync();
    }

    public async Task<EmployeeResponse?> GetByIdAsync(int id)
    {
        var result = await context.Employees.Where(e => e.Id == id)
            .Select(t => new EmployeeResponse(t.Id, t.Name, t.Salary.Salary, t.JoinedDate))
            .FirstOrDefaultAsync();
        return result;
    }

    public async Task<EmployeeResponse> CreateAsync(CreateEmployeeRequest request)
    {
        Employee employee = new()
        {
            Name = request.Name,
            DepartmentId = request.DepartmentId,
            JoinedDate = request.JoinedDate,
            Salary = new Salaries { Salary = request.SalaryAmount }
        };

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return new EmployeeResponse(
            employee.Id,
            employee.Name,
            employee.Salary.Salary,
            employee.JoinedDate);
    }

    public async Task<EmployeeResponse?> UpdateAsync(int id, UpdateEmployeeRequest request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();
        try
        {
            var existingEmployee = await context.Employees.FindAsync(id);

            if (existingEmployee is null)
            {
                return null;
            }

            existingEmployee.Name = request.Name;
            existingEmployee.DepartmentId = request.DepartmentId;
            existingEmployee.JoinedDate = request.JoinedDate;
            existingEmployee.Salary.Salary = request.SalaryAmount;

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new EmployeeResponse(
                existingEmployee.Id,
                existingEmployee.Name,
                existingEmployee.Salary.Salary,
                existingEmployee.JoinedDate);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existingEmployee = await context.Employees.FindAsync(id);
        if (existingEmployee is null)
        {
            return false;
        }

        context.Employees.Remove(existingEmployee);
        await context.SaveChangesAsync();
        return true;
    }

    private async Task<List<EmployeesWithDepartments>> GetEmployeesWithDepartmentsSqlAsync()
    {
        List<EmployeesWithDepartments> result = await context.Database.SqlQuery<EmployeesWithDepartments>(
                $"""
                 SELECT
                     e.Id,
                     e.Name AS FullName,
                     d.Name AS DepartmentName
                 FROM
                     Employees e
                         LEFT JOIN
                     Departments d ON e.DepartmentId = d.Id
                 ORDER BY
                     e.Name, d.Name
                 """)
            .ToListAsync();
        return result;
    }

    private async Task<List<EmployeesWithDepartments>> GetEmployeesWithDepartmentsLinqAsync()
    {
        List<EmployeesWithDepartments> result = await context.Employees
            .Join(context.Departments, e => e.DepartmentId, d => d.Id,
                (employee, department) => new EmployeesWithDepartments(
                    employee.Id,
                    employee.Name,
                    department.Name
                )
            )
            .AsNoTracking()
            .ToListAsync();
        return result;
    }

    private async Task<List<EmployeesWithProjects>> GetEmployeesWithProjectsSqlAsync()
    {
        List<EmployeesWithProjects> result = await context.Database.SqlQuery<EmployeesWithProjects>(
                $"""
                 SELECT
                     e.Id AS Id,
                     e.Name AS FullName,
                     p.Name AS ProjectName
                 FROM
                     Employees e
                         LEFT JOIN
                     ProjectEmployee pe ON e.Id = pe.EmployeeId AND pe.Enable = 1
                         LEFT JOIN
                     Projects p ON pe.ProjectId = p.Id
                 ORDER BY
                     e.Name, p.Name
                 """)
            .ToListAsync();
        return result;
    }

    private async Task<List<EmployeeResponse>> GetHighPaidRecentEmployeesLinqAsync()
    {
        List<EmployeeResponse> result = await context.Employees
            .Include(t => t.Salary)
            .Where(t => t.Salary.Salary > 100 && t.JoinedDate >= new DateOnly(2024, 1, 1))
            .Select(t => new EmployeeResponse(t.Id, t.Name, t.Salary.Salary, t.JoinedDate))
            .AsNoTracking()
            .ToListAsync();
        return result;
    }

    private async Task<List<EmployeeResponse>> GetHighPaidRecentEmployeesSqlAsync()
    {
        List<EmployeeResponse> result = await context.Database.SqlQuery<EmployeeResponse>(
                $"""
                 SELECT [e].[Id], 
                        [e].[Name] AS FullName, 
                        [s].[Salary] AS SalaryAmount, 
                        [e].[JoinedDate]
                 FROM [Employees] AS [e]
                 LEFT JOIN [Salaries] AS [s] ON [e].[Id] = [s].[EmployeeId]
                 WHERE [s].[Salary] > 100 AND [e].[JoinedDate] >= '2024-01-01'
                 """)
            .ToListAsync();
        return result;
    }
}