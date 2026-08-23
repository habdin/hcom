using System;
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

	// Define db context for the App.
	private TestContext context = default!;

	// Single Item record for the target models.
	public Dummy2 Item { get; set; } = new Dummy2();
	public Category CategoryItem { get; set; } = new Category();

	// Define form EditContexts for the existing Model instances
	public EditContext? editcontext { get; set; }
	public EditContext? categoryEditContext { get; set; }

	// Paginator related fields and properties
	private int PageSize = 10;
	private int _totalRecords;
	private int _totalPages;
	private int _currentPage = 1;

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

	private async Task PageAmountChangedHandler(int size)
	{
		// Assigns the value received from the event run to PageSize
		PageSize = size;
		// Refresh the database entries accordingly
		await LoadItemsAsync();
	}

	private async Task PagerLinkClickHandler(int page)
	{
		_currentPage = page;
		await LoadItemsAsync();
	}

	private async Task LoadItemsAsync()
	{
		IsLoading = true;
		try
		{
			// Initially define the query
			IQueryable<Dummy2> query = context.Dummy2;

			// Change the query according to the Search string
			if (!string.IsNullOrWhiteSpace(FilterString))
			{
				query = query.Where(d =>
							EF.Functions.Like(d.Name, $"%{FilterString}%"));
			}

			// Catch the totalRecords at the db level
			_totalRecords = await query.CountAsync();

			// Internally calculate the totalPages 
			_totalPages = (int)Math.Ceiling((double)_totalRecords / PageSize);

			// Safety net: Guard against stale state from deletions or filtering
			_currentPage = _totalPages > 0
				? Math.Clamp(_currentPage, 1, _totalPages)
				: 1;

			// Slice the query only when the result spans multiple pages.
			if (_totalPages > 1)
			{
				Items = await query
					.Include(d => d.Category)
					.OrderBy(d => d.Id)
					.Skip((_currentPage - 1) * PageSize)
					.Take(PageSize)
					.ToListAsync();
			}
			// Here the query will already be catching the fitterString
			// so no need to push the Where for filtering.
			else Items = await query
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
	}

	// Uncomment DisposeAsync method
	public async ValueTask DisposeAsync() => await context.DisposeAsync();

	// ===== End of Methods Section ==== //
}
