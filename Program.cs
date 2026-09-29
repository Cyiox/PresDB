using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using WebPresDB.Components;
using WebPresDB.Models;
using WebPresDB.Services;

var builder = WebApplication.CreateBuilder(args);

// Authentication & Authorization Services 
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ExpUseDB_Read", policy =>
        policy.RequireRole ("ExpUseDB_Read","ExpUseDB_Edit", "ExpUseDB_Admin"));
        
    options.AddPolicy("ExpUseDB_Edit", policy =>
        policy.RequireRole("ExpUseDB_Edit", "ExpUseDB_Admin"));

    options.AddPolicy("ExpUseDB_Admin", policy =>
        policy.RequireRole("ExpUseDB_Admin"));
});

// Required to serve the sign-in/sign-out endpoints
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();

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

// Auth Middleware (Must be before UseAntiforgery)
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();

//  Map Sign-in / Sign-out routes 
app.MapControllers();



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

static string GetDatabaseConnectionString(IConfiguration configuration)
{
    var connectionName = configuration["Database:ConnectionStringName"] ?? "Default";
    return configuration.GetConnectionString(connectionName)
        ?? throw new InvalidOperationException($"ConnectionStrings:{connectionName} is not configured.");
}
