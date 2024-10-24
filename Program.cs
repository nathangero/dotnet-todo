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

app.Run();
