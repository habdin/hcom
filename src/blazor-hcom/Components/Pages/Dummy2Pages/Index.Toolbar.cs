using Microsoft.AspNetCore.Components;

// Update the Model entity namespace as needed.
namespace blazor_hcom.Components.Pages.Dummy2Pages;

public partial class Index : ComponentBase, IAsyncDisposable
{
	// Boolean field defined to help Data interface toggling ability 
	// provided by <Toolbar /> component
	public bool ViewAsTable { get; set; } = false;

	// Field used in LoadItemsAsync to implement the Search ability
	// provided by <Toolbar /> component.
	private string? FilterString { get; set; } = "";

	// <Toolbar /> component Search Ability Handler
	private async Task SearchHandler(string? value)
	{
		FilterString = value;
		await LoadItemsAsync();
	}

	// <Toolbar /> component Data toggling Ability Handler
	private void HandleToggleView(bool obj)
	{
		ViewAsTable = obj;
	}
}
