using AlfieTodoList.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

// Registers the Entity Framework Core DbContext class with the .NET dependency injection container
var connection = String.Empty;
if (builder.Environment.IsDevelopment())
{
    System.Diagnostics.Debug.WriteLine("IsDevelopment");
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
    connection = builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
}
else
{
    System.Diagnostics.Debug.WriteLine("GetEnvironmentVariable");
    connection = Environment.GetEnvironmentVariable("AZURE_SQL_CONNECTIONSTRING");
}

System.Diagnostics.Debug.WriteLine(connection);
builder.Services.AddDbContext<TodoItemDbContext>(options =>
    options.UseSqlServer(connection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TodoItems}/{action=Index}/{id?}");

app.Run();

public class TodoItemDbContext : DbContext
{
    public TodoItemDbContext(DbContextOptions<TodoItemDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItem { get; set; }
}

