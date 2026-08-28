using Microsoft.AspNetCore.Components;

// Update the Model entity namespace as needed.
using blazor_hcom.Models;

using blazor_hcom.Classes;
using blazor_hcom.Components.Layout;
using System;

// Change the namespace for the App as required.
namespace blazor_hcom.Components.Pages.Dummy2Pages;

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
			Item = new Dummy2();
			editcontext = CreateEditContext(Item);
			if (Env.IsDevelopment())
			{
				Logger?.LogInformation(
				    $"Variable {nameof(editcontext)} reassigned by {nameof(HandleCreate)}");
			}
			CrudOperation = "create";
			ModalTitle = GetModalTitle(CrudOperation, Item);
			ModalSubmitBtnText = "Create";
			await ModalShowAsync<Dummy2>(_dummy2ModalRef!);
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error during HandleCreate");
			await NotifySrvs.AddMessage("Error during creating a new Dummy2. Please try again", MessageLevel.Error);
		}
	}

	private async Task HandleCreateCategory()
	{
		try
		{
			CategoryItem = new Category();
			categoryEditContext = CategoryCreateEditContext(CategoryItem);
			if (Env.IsDevelopment())
			{
				Logger?.LogInformation(
				    $"Variable {nameof(categoryEditContext)} reassigned by {nameof(HandleCreateCategory)}");
			}
			CrudOperation = "create";
			ModalTitle = "Add new Record";
			ModalSubmitBtnText = "Create";
			await ModalHideAsync<Dummy2>(_dummy2ModalRef!);
			await ModalShowAsync<Category>(_categoryModalRef!);
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error during HandleCreateCategory");
			await NotifySrvs.AddMessage("Error during creating a new Category. Please try again", MessageLevel.Error);
		}
	}

	private async Task HandleRead(Dummy2 item)
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
			await ModalShowAsync<Dummy2>(_dummy2ModalRef!);
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error during HandleRead");
			await NotifySrvs.AddMessage($"An error occured while reading.", MessageLevel.Error);
		}
	}

	private async Task HandleUpdate(Dummy2 item)
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
			if (Env.IsDevelopment())
			{
				Logger?.LogInformation(
				    $"Variable {nameof(editcontext)} reassigned by {nameof(HandleUpdate)}");
			}
			CrudOperation = "update";
			ModalTitle = GetModalTitle(CrudOperation, Item);
			ModalSubmitBtnText = "Update";
			await ModalShowAsync<Dummy2>(_dummy2ModalRef!);
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error during HandleUpdate");
			await NotifySrvs.AddMessage("Error during updating item.", MessageLevel.Error);
		}
	}

	private async Task HandleDelete(Dummy2 item)
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
			await ModalShowAsync<Dummy2>(_dummy2ModalRef!);
		}
		catch (Exception ex)
		{
			Logger?.LogError(ex, "Error during HandleDelete");
			await NotifySrvs.AddMessage("Error during deleting item.", MessageLevel.Error);
		}
	}
}
