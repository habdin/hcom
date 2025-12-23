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

    // The following 4 helper methods are event handlers that handle events
    // emitted for different CRUD operations via the UI.
	// Submit handler for Modal Submit button click event.
	// The main pivot function for all CRUD operations.
    private async Task HandleCreate () {
		try {
			Item = new Dummy();
			editcontext = CreateEditContext(Item);
			CrudOperation = "create";
			ModalTitle = GetModalTitle(CrudOperation, Item);
			ModalSubmitBtnText = "Create";
			await ShowModalAsync();
		}
		catch (Exception ex){
			Logger?.LogError(ex, "Error during HandleCreate");
            await NotifySrvs.AddMessage("Error during creating a new Dummy. Please try again", MessageLevel.Error);
		}
	}

	private async Task HandleRead (Dummy item) {
		try {
			if (item == null)
			{
				Logger.LogWarning("Error during HandleRead: item is null");
				return;
			}
			Item = item;
			CrudOperation = "read";
			ModalTitle = GetModalTitle(CrudOperation, Item);
			ModalSubmitBtnText = "Dismiss";
			await ShowModalAsync();
		}
		catch (Exception ex){
			Logger?.LogError(ex, "Error during HandleRead");
            await NotifySrvs.AddMessage($"An error occured while reading.", MessageLevel.Error);
        }
	}

	private async Task HandleUpdate (Dummy item)
	{
		try {
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
			await ShowModalAsync();
		}
		catch (Exception ex){
			Logger?.LogError(ex, "Error during HandleUpdate");
            await NotifySrvs.AddMessage("Error during updating item.", MessageLevel.Error);
        }
	}

	private async Task HandleDelete (Dummy item) {
		try {
			if (item == null)
			{
				Logger.LogWarning("Error during HandleDelete: item is null");
				return;
			}
			Item = item;
			CrudOperation = "delete";
			ModalTitle = GetModalTitle(CrudOperation, Item);
			ModalSubmitBtnText = "Delete";
			await ShowModalAsync();
		}
		catch (Exception ex){
			Logger?.LogError(ex, "Error during HandleDelete");
            await NotifySrvs.AddMessage("Error during deleting item.", MessageLevel.Error);
        }
	}
}
