using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace blazor_hcom.Models;

public class Dummy {
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	[Required]
	[MinLength(2)]
	[MaxLength(65)]
	public string? Name { get; set; }

	public string? Category { get; set; }
}
