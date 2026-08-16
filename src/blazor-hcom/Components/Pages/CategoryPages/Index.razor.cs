using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using blazor_hcom.Models.Dummy2;
using blazor_hcom.Services;

// Change the namespace for the App as required
namespace blazor_hcom.Components.Pages.CategoryPages;

public partial class Index : ComponentBase, IAsyncDisposable
{
	// ====== Fields and Properties Section ====== //

	// Inject the HCOM DBContext Factory
	[Inject] IDbContextFactory<TestContext> DbFactory { get; set; } = default!;
	// Inject the host environment. For use in possible development conditions.
	[Inject] IHostEnvironment Env { get; set; } = default!;
	// Inject the logging for the App Index page.
	[Inject] private ILogger<Index> Logger { get; set; } = default!;
	// Inject HCOM MessageService.
	[Inject] IMessageService NotifySrvs { get; set; } = default!;
	// Inject HCOM UiStateService 
	[Inject] UiStateService UiStateSrvs { get; set; } = default!;

	// Define context for the App.
	private TestContext context = default!;
	// Define App for EditContext.
	// It is pushed in this component since it is a general purpose component partial file
	// for the Modal and CRUD as well as for the forms inside them.
	private EditContext? editcontext = default!;

	// Boolean variable used while data loading is in progress.
	// Used in LoadItemsAsync.
	public bool IsLoading { get; set; }

	// Created as private since it is only used in this class
	private List<Category> Items = new();

	// Created as public since it will be used elsewhere in the project
	// Single Item record for the target model.
	public Category Item { get; set; } = new Category();
	
	// ===== End of Fields and Properties Section ===== //

	// ===== Methods Section ===== //

	// ----- Helper methods subsection ----- //
	// Uncomment LoadItemsAsync and modify it as needed.
	private async Task LoadItemsAsync()
	{
		IsLoading = true;
		try
		{
			Items = string.IsNullOrWhiteSpace(FilterString)
			    ? await context.Category.ToListAsync()
			    : await context.Category
			    // Case sensitive method
			    // .Where(d=>d.name.Contains(FilterString))
			    // Case Insensitive method
			    .Where(d => EF.Functions.Like(d.Name, $"%{FilterString}%"))
			    .ToListAsync();
		}
		finally
		{
			IsLoading = false;
		}
	}

	// JS-based helper methods used to open/close Modal.
	// private async Task ShowModalAsync() => await JS.InvokeVoidAsync("showModal", "theModal");

	// private async Task HideModalAsync() => await JS.InvokeVoidAsync("hideModal", "theModal");


	// -------- End of Helper methods subsection -------- //

	protected override async Task OnInitializedAsync()
	{
		context = DbFactory.CreateDbContext();
		await LoadItemsAsync();
		StateHasChanged();
	}

	public async ValueTask DisposeAsync() => await context.DisposeAsync();

	// ===== End of Methods Section ==== //
}
