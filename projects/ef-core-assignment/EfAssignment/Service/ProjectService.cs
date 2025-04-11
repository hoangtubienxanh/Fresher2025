using EfAssignment.Ef;
using EfAssignment.Ef.Models;
using EfAssignment.Models;

using Microsoft.EntityFrameworkCore;

namespace EfAssignment.Service;

public class ProjectService(OneDbContext context) : IProjectService
{
    public async Task<List<Project>> GetAsync()
    {
        return await context.Projects.ToListAsync();
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        return await context.Projects.FindAsync(id);
    }

    public async Task<Project> CreateAsync(CreateProjectRequest request)
    {
        Project project = new() { Name = request.Name };

        context.Projects.Add(project);
        await context.SaveChangesAsync();

        return project;
    }

    public async Task<Project?> UpdateAsync(int id, UpdateProjectRequest request)
    {
        var project = await context.Projects.FindAsync(id);
        if (project == null)
        {
            return null;
        }

        project.Name = request.Name;
        await context.SaveChangesAsync();

        return project;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await context.Projects.FindAsync(id);
        if (project == null)
        {
            return false;
        }

        context.Projects.Remove(project);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EnrollEmployeeAsync(int projectId, EnrollEmployeeRequest request)
    {
        var project = await context.Projects.FindAsync(projectId);
        if (project == null)
        {
            return false;
        }

        var employee = await context.Employees.FindAsync(request.EmployeeId);
        if (employee == null)
        {
            return false;
        }

        var existingEnrollment = await context.Set<ProjectEmployee>()
            .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == request.EmployeeId);

        if (existingEnrollment != null)
        {
            existingEnrollment.Enable = request.Enable;
        }
        else
        {
            ProjectEmployee projectEmployee = new()
            {
                ProjectId = projectId, EmployeeId = request.EmployeeId, Enable = request.Enable
            };
            await context.Set<ProjectEmployee>().AddAsync(projectEmployee);
        }

        await context.SaveChangesAsync();
        return true;
    }
}