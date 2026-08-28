using Microsoft.AspNetCore.Components;

// Update the Model entity namespace as needed.
using blazor_hcom.Models;

using blazor_hcom.Classes;

// Change the namespace for the App as required.
namespace blazor_hcom.Components.Pages.CategoryPages;

public partial class Index : ComponentBase, IAsyncDisposable
{

	// The following 4 helper methods are event handlers that handle events
	// emitted for different CRUD operations via the UI.
	// Submit handler for Modal Submit button click event.
	// The main pivot function for all CRUD operations.
	private async Task HandleCreate()
	{
		try
		{
			// Change the Model Entity class name.
			Item = new Category();
			editcontext = CreateEditContext(Item);
			CrudOperation = "create";
			ModalTitle = GetModalTitle(CrudOperation, Item);
			ModalSubmitBtnText = "Create";
			await ModalShowAsync();
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error during HandleCreate");
			await NotifySrvs.AddMessage("Error during creating a new Category. Please try again", MessageLevel.Error);
		}
	}

	private async Task HandleRead(Category item)
	{
		try
		{
			if (item == null)
			{
				Logger.LogWarning("Error during HandleRead: item is null");
				return;
			}
			Item = item;
			CrudOperation = "read";
			ModalTitle = GetModalTitle(CrudOperation, Item);
			ModalSubmitBtnText = "Dismiss";
			await ModalShowAsync();
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error during HandleRead");
			await NotifySrvs.AddMessage($"An error occured while reading.", MessageLevel.Error);
		}
	}

	private async Task HandleUpdate(Category item)
	{
		try
		{
			if (item == null)
			{
				Logger.LogWarning("Error during HandleUpdate: item is null");
				return;
			}
			Item = item;
			editcontext = CreateEditContext(Item);
			CrudOperation = "update";
			ModalTitle = GetModalTitle(CrudOperation, Item);
			ModalSubmitBtnText = "Update";
			await ModalShowAsync();
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error during HandleUpdate");
			await NotifySrvs.AddMessage("Error during updating item.", MessageLevel.Error);
		}
	}

	private async Task HandleDelete(Category item)
	{
		try
		{
			if (item == null)
			{
				Logger.LogWarning("Error during HandleDelete: item is null");
				return;
			}
			Item = item;
			CrudOperation = "delete";
			ModalTitle = GetModalTitle(CrudOperation, Item);
			ModalSubmitBtnText = "Delete";
			await ModalShowAsync();
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error during HandleDelete");
			await NotifySrvs.AddMessage("Error during deleting item.", MessageLevel.Error);
		}
	}
}
