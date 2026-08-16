using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using blazor_hcom.Models.Dummy2;
using blazor_hcom.Services;

namespace blazor_hcom.Components.Pages.Dummy2Pages;

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
	// Created as public since it will be used elsewhere in the project
	// Single Item record for the target model.
	public Dummy2 Item { get; set; } = new Dummy2();
	public Category CategoryItem { get; set; } = new Category();

	// for the Modal and CRUD as well as for the forms inside them.
	public EditContext? editcontext { get; set; }
	public EditContext? categoryEditContext { get; set; }

	// Boolean variable used while data loading is in progress.
	// Used in LoadItemsAsync.
	public bool IsLoading { get; set; }

	// Created as private since it is only used in this class
	private List<Dummy2> Items = new();
	private List<Category> CategoryItems = new();

	// ===== End of Fields and Properties Section ===== //

	// ===== Methods Section ===== //

	// ----- Helper methods subsection ----- //

	private async Task DepModalOnCloseHandler()
	{
		// Hide the Dependent Modal via its reference
		await ModalHideAsync<Category>(_categoryModalRef!);

		// Show the Primary Modal via its reference
		await ModalShowAsync<Dummy2>(_dummy2ModalRef!);
	}

	private async Task DepModalPostSubmissionHandler()
	{
		// Refresh the category items from database.
		await LoadCategoryItemsAsync();

		// Invoke re-render.
		await InvokeAsync(StateHasChanged);

		// Perform dep modal hiding/ primary modal showing sequence
		await DepModalOnCloseHandler();
	}

	private async Task LoadItemsAsync()
	{
		IsLoading = true;
		try
		{
			Items = string.IsNullOrWhiteSpace(FilterString)
			    ? await context.Dummy2
			    .Include(d => d.Category)
			    .ToListAsync()
			    : await context.Dummy2
			    // Case sensitive method
			    // .Where(d=>d.name.Contains(FilterString))
			    // Case Insensitive method
			    .Where(d => EF.Functions.Like(d.Name, $"%{FilterString}%"))
			    .Include(d => d.Category)
			    .ToListAsync();
		}
		finally
		{
			IsLoading = false;
		}
	}

	private async Task LoadCategoryItemsAsync()
	{
		IsLoading = true;
		try
		{
			CategoryItems = string.IsNullOrWhiteSpace(FilterString)
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
		// Push EditContext default values for corresponding Forms EditForm
		// EditContexts are needed for Modal rendering even if Modals are hidden.
		editcontext = new(Item);
		categoryEditContext = new(CategoryItem);
		if (Env.IsDevelopment())
		{
			Logger?.LogInformation(
			    $"{nameof(editcontext)} has been initialized.");

			Logger?.LogInformation(
			    $"{nameof(categoryEditContext)} has been initialized.");
		}
		await LoadItemsAsync();
		await LoadCategoryItemsAsync();
		StateHasChanged();
	}

	// Uncomment DisposeAsync method
	public async ValueTask DisposeAsync() => await context.DisposeAsync();

	// ===== End of Methods Section ==== //
}
