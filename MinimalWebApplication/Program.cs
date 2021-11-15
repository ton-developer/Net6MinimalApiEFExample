using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using MinimalWebApplication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MySql Database
builder.Services.AddDbContext<CustomDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 0))));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// App
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

// Endpoints
app.MapGet("/List", async (CustomDbContext dbContext) =>
{
    var stopWatchQuery = new Stopwatch();
    stopWatchQuery.Start();
    var listOfObjectsFromDb = await dbContext.Objects.ToListAsync();
    stopWatchQuery.Stop();
    return Results.Ok(listOfObjectsFromDb.Select(x => new
    {
        x.Id,
        x.Name,
        Yers = x.Years,
        stopWatchQuery.ElapsedMilliseconds
    }));
});

app.MapPost("/Insert", async (string name, int years, CustomDbContext dbContext) =>
{
    await dbContext.Objects.AddAsync(new DbObject {Name = name, Years = years});
    await dbContext.SaveChangesAsync();
});

app.Run();