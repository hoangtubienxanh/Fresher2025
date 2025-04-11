using EfAssignment.Api;
using EfAssignment.Ef;
using EfAssignment.Service;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();

builder.Services.AddDbContext<OneDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("assignment-one-db"))
        .UseSeeding(SeedData.Seed);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map all endpoints
app.MapEmployeeEndpoints();
app.MapProjectEndpoints();
app.MapDepartmentEndpoints();

app.Run();