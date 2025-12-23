// Aim: Test Dummy Index for Loading Records from backend DB
// Method: Implement a bUnit test for Testing Index component to show
// the in-memory database entries.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using blazor_hcom.Models;
using blazor_hcom.Services;
using DummyIndex = blazor_hcom.Components.Pages.DummyPages.Index;
using Bunit;

namespace HCOM.Tests;

public class DummyIndexLoadDataTests
{
    [Fact]
    public void DummyIndexShowsDBRecords()
    {
        // Arrange
        using var cntxt = new BunitContext();
        // This is injected in the original Index.razor with OnInitializedAsync
		
        cntxt.Services.AddSingleton<IMessageService, NotificationService>();
        cntxt.Services.AddSingleton<IHostEnvironment>(new FakeHostEnvironment());
        cntxt.JSInterop.Mode = JSRuntimeMode.Loose;
        cntxt.Services.AddLogging();
        cntxt.Services.AddDbContextFactory<TestContext>(
            options => options.UseInMemoryDatabase("DummyIndexTestDb"));

        using (var scope = cntxt.Services
			   .GetRequiredService<IDbContextFactory<TestContext>>()
               .CreateDbContext())
        {
            scope.Database.EnsureDeleted();
            scope.Database.EnsureCreated();

            scope.AddRange(
              new Dummy { Id = 1, Name = "Dummy A", Category = "Cat1" },
              new Dummy { Id = 2, Name = "Dummy B", Category = "Cat2" },
              new Dummy { Id = 3, Name = "Dummy C", Category = "Cat1" }
            );
            scope.SaveChanges();
        }


        // Act
        var cut = cntxt.Render<DummyIndex>();
        cut.WaitForAssertion(() =>
        {
            var cards = cut.FindAll(".card");
            Assert.Equal(3, cards.Count);
            Assert.Contains("Dummy A", cut.Markup);
            Assert.Contains("Dummy B", cut.Markup);
            Assert.Contains("Dummy C", cut.Markup);
        }, TimeSpan.FromSeconds(5));
    }
}

public class FakeHostEnvironment : IHostEnvironment {
    public string EnvironmentName { get; set; } = Environments.Development;
    public string ApplicationName { get; set; } = "TestApp";
    public string ContentRootPath { get; set; } = "";
    public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
}

