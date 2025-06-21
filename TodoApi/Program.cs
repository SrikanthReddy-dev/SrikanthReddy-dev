using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var tasks = new List<TodoItem>
{
    new TodoItem { Id = 1, Title = "Sample Task", IsComplete = false }
};

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/tasks", () => tasks);

app.MapGet("/tasks/{id}", (int id) =>
    tasks.FirstOrDefault(t => t.Id == id) is TodoItem task ? Results.Ok(task) : Results.NotFound());

app.MapPost("/tasks", ([FromBody] TodoItem task) =>
{
    task.Id = tasks.Max(t => t.Id) + 1;
    tasks.Add(task);
    return Results.Created($"/tasks/{task.Id}", task);
});

app.Run();

class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public bool IsComplete { get; set; }
}
