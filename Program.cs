using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

var KEY_TODO_TITLE = "title";
var KEY_TODO_COMPLETION = "completed";
var DB = new Dictionary<int, Dictionary<string, object>>();
int index = 1;

app.MapGet("/", () => "Welcome to the dotnet server");

app.MapGet("/allTodos", () =>
{
    if (DB.Count.Equals(0))
    {
        return Results.Json("No todos found");
    }
    else
    {
        return Results.Json(DB);
    }
});

app.MapPut("/todo", async (HttpContext context) =>
{
    var body = await context.Request.ReadFromJsonAsync<Dictionary<string, string>>();
    var todo = body["todo"];

    // Console.WriteLine(todo);

    DB[index] = new Dictionary<string, object>{
        {KEY_TODO_TITLE, todo},
        {KEY_TODO_COMPLETION, false}, // A new todo is always not completed
    };
    index++;

    return Results.Json(DB);
});

app.MapPatch("/todo", async (HttpContext context) =>
{
    var body = await context.Request.ReadFromJsonAsync<Dictionary<string, object>>();

    var todoIndex = ((JsonElement)body["index"]).GetInt32();
    // Console.WriteLine(todoIndex);

    var todoName = body.ContainsKey("todoName") ? body["todoName"].ToString() : null;

    var completed = ((JsonElement)body["completed"]).GetBoolean();
    // Console.WriteLine(completed);

    if (todoName != null)
    {
        DB[todoIndex][KEY_TODO_TITLE] = todoName;
    }

    DB[todoIndex][KEY_TODO_COMPLETION] = completed;
    return Results.Json(DB[todoIndex]);
});

app.Run();
