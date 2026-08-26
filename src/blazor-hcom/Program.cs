// EF Core infrastructure:
// DbContextFactory<T>, SQLite provider, database configuration.
using Microsoft.EntityFrameworkCore;

// Root HCOM UI component (App.razor).
using blazor_hcom.Components;

// Implicit Dependency injection imports exist so the import is commented.
// using Microsoft.Extensions.DependencyInjection;

// HCOM application services (notifications, UI state, etc.).
using blazor_hcom.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<TestContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TestContext") ?? throw new InvalidOperationException("Connection string 'TestContext' not found.")));

builder.Services.AddScoped<IMessageService, NotificationService>();
builder.Services.AddScoped<UiStateService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
