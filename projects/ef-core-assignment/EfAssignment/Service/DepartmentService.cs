using EfAssignment.Ef;
using EfAssignment.Ef.Models;
using EfAssignment.Models;

using Microsoft.EntityFrameworkCore;

namespace EfAssignment.Service;

public class DepartmentService(OneDbContext context) : IDepartmentService
{
    public async Task<List<Department>> GetAsync()
    {
        return await context.Departments.ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await context.Departments.FindAsync(id);
    }

    public async Task<Department> CreateAsync(CreateDepartmentRequest request)
    {
        Department department = new() { Name = request.Name };

        context.Departments.Add(department);
        await context.SaveChangesAsync();

        return department;
    }

    public async Task<Department?> UpdateAsync(int id, UpdateDepartmentRequest request)
    {
        var department = await context.Departments.FindAsync(id);
        if (department == null)
        {
            return null;
        }

        department.Name = request.Name;
        await context.SaveChangesAsync();

        return department;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await context.Departments.FindAsync(id);
        if (department == null)
        {
            return false;
        }

        context.Departments.Remove(department);
        await context.SaveChangesAsync();
        return true;
    }
}