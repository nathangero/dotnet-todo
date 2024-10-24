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

app.UseHttpsRedirection();

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

app.MapPut("/todo", (HttpContext context) =>
{
    var body = context.Request.ReadFromJsonAsync<Dictionary<string, string>>().Result;
    var todo = body["todo"];

    // Console.WriteLine(todo);

    DB[index] = new Dictionary<string, object>{
        {KEY_TODO_TITLE, todo},
        {KEY_TODO_COMPLETION, false}, // A new todo is always not completed
    };
    index++;

    return Results.Json(DB);
});

app.Run();
