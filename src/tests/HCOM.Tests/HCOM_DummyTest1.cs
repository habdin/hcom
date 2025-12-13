using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using blazor_hcom.Models;

namespace HCOM.Tests;

public class TestContextUnitTests
{
    private TestContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<TestContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        var context = new TestContext(options);

        context.Dummy.AddRange(
            new Dummy { Id = 1, Name = "Dummy A", Category = "Cat1" },
            new Dummy { Id = 2, Name = "Dummy B", Category = "Cat2" },
            new Dummy { Id = 3, Name = "Dummy C", Category = "Cat1" }
        );

        context.SaveChanges();

        return context;
    }


    [Fact]
    public void Context_ShouldReturnDummyItems()
    {
        var context = GetInMemoryContext();

        var dummies = context.Dummy.Take(10).ToList();

        Assert.Equal(3, dummies.Count);
        Assert.Contains(dummies, d => d.Name == "Dummy A");
        Assert.Contains(dummies, d => d.Name == "Dummy B");
        Assert.Contains(dummies, d => d.Category == "Cat1");
    }
}
