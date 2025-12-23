using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using blazor_hcom.Models;
using blazor_hcom.Services;
using blazor_hcom.Classes;


namespace blazor_hcom.Components.Pages.DummyPages;

public partial class Index : ComponentBase, IAsyncDisposable
{
	// ====== Fields and Properties Section ====== //

    [Inject] IDbContextFactory<TestContext> DbFactory { get; set; } = default!;
    [Inject] IHostEnvironment Env { get; set; } = default!;
    [Inject] IJSRuntime JS { get; set; } = default!;
	[Inject] private ILogger<Index> Logger { get; set; } = default!;
    [Inject] IMessageService NotifySrvs { get; set; } = default!;

    private TestContext context = default!;
    private EditContext? editcontext = default!;

    public bool IsLoading { get; set; }
    // Created as private since it is only used in this class
    private List<Dummy> Items = new();

	// Created as public since it will be used elsewhere in the project
    public Dummy Item { get; set; } = new Dummy();


    // ===== End of Fields and Properties Section ===== //

    // ===== Methods Section ===== //

	// ----- Helper methods subsection ----- //
    private async Task LoadItemsAsync()
    {
        IsLoading = true;
		try {
			Items = string.IsNullOrWhiteSpace(FilterString)
				? await context.Dummy.ToListAsync()
				: await context.Dummy
					.Where(d => d.Name!.Contains(FilterString))
					.ToListAsync();
		}
		finally{
            IsLoading = false;
        }
    }

	// JS-based helper methods used to open/close Modal.
	private async Task ShowModalAsync() => await JS.InvokeVoidAsync("showModal", "theModal");
	private async Task HideModalAsync() => await JS.InvokeVoidAsync("hideModal", "theModal");

	
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
