using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using blazor_hcom.Data;

using blazor_hcom.Models;

public class TestContext : IdentityDbContext<ApplicationUser>
{
	public TestContext(DbContextOptions<TestContext> options)
	    : base(options)
	{
	}
	public DbSet<Dummy> Dummy { get; set; } = default!;
	public DbSet<Dummy2> Dummy2 { get; set; } = default!;
	public DbSet<Category> Category { get; set; } = default!;
}
