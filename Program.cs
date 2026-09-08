using Microsoft.EntityFrameworkCore;
using WebPresDB.Components;
using WebPresDB.Models;
using WebPresDB.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddDbContext<PreservationTestContext>(options =>
    options.UseSqlServer(GetDatabaseConnectionString(builder.Configuration)));

builder.Services.AddScoped<IPropertyService, PropertyService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

static string GetDatabaseConnectionString(IConfiguration configuration)
{
    var connectionName = configuration["Database:ConnectionStringName"] ?? "Default";
    return configuration.GetConnectionString(connectionName)
        ?? throw new InvalidOperationException($"ConnectionStrings:{connectionName} is not configured.");
}
