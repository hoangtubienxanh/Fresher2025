using ApiAssignment.Apis;
using ApiAssignment.Infrastructure;
using ApiAssignment.Infrastructure.Repositories;
using ApiAssignment.Services;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TodoTaskDbContext>(dbContextOptionsBuilder =>
{
    dbContextOptionsBuilder.UseSqlite(builder.Configuration.GetConnectionString(nameof(TodoTaskDbContext)));
    dbContextOptionsBuilder.UseSeeding(SeedData.Seed).UseAsyncSeeding(SeedData.SeedAsync);
});

builder.Services.AddScoped<ITodoTaskService, TodoTaskService>();
builder.Services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapTaskEndpoints();
app.MapPersonEndpoints();

app.Run();

var services = app.Services;
await using var context = services.GetRequiredService<TodoTaskDbContext>();
context.Database.EnsureCreated();