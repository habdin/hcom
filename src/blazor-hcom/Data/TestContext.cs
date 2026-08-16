using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using blazor_hcom.Models;
using blazor_hcom.Models.Dummy2;

public class TestContext : DbContext
{
	public TestContext(DbContextOptions<TestContext> options)
	    : base(options)
	{
	}

	public DbSet<Dummy> Dummy { get; set; } = default!;
	public DbSet<Dummy2> Dummy2 { get; set; } = default!;
	public DbSet<Category> Category { get; set; } = default!;
}
