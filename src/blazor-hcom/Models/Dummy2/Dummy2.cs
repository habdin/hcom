using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blazor_hcom.Models;

public class Dummy2
{
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[Required]
	[MinLength(2)]
	[MaxLength(65)]
	public string? Name { get; set; }

	[ForeignKey("Category")]
	[Display(Name = "Category Id", AutoGenerateField = false)]
	public int CategoryId { get; set; }
	public virtual Category? Category { get; set; }
}

