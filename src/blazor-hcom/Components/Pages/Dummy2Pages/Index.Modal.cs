using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

using blazor_hcom.Models;

using blazor_hcom.Classes;
using blazor_hcom.Components.Layout;

namespace blazor_hcom.Components.Pages.Dummy2Pages;

public partial class Index : ComponentBase, IAsyncDisposable
{
	// Modal Related properties that used to tweak the <Modal />

	// Modal reference for use in related modal component.
	private Modal<Dummy2>? _dummy2ModalRef;
	private Modal<Category>? _categoryModalRef;
	private string CrudOperation { get; set; } = "";
	private string ModalSubmitBtnText { get; set; } = "Save";
	private string ModalTitle { get; set; } = "";

	// Modal methods
	private static Task HandleBackdropClick<TEntity>(Modal<TEntity> modal) => modal.Hide();
	private static Task ModalShowAsync<TEntity>(Modal<TEntity> modal) => modal.Show();
	private static Task ModalHideAsync<TEntity>(Modal<TEntity> modal) => modal.Hide();

	private void HandleModalVisibility(bool visible)
	{
		// Since the modal catches the state of modal visibility which
		// is changed via calling a method to show the modal and that
		// internally changes the OnVisibilityChanged.
		// The Role of the method is to catch this state change and change
		// the service state. This will be used to toggle the
		// overflow:hidden state in the body tag in App.razor
		UiStateSrvs.ModalOpen = visible;
	}

	private RenderFragment MainForm(string operation) => operation switch
	{
		"create" or "update" => builder =>
		{
			builder.OpenComponent(0, typeof(ModelCreateUpdateForm));
			builder.AddAttribute(1, "Dummy2", Item);
			builder.AddAttribute(2, "Operation", operation);
			builder.AddAttribute(3, "EditContext", editcontext);
			builder.AddAttribute(4, "CategoryItems", CategoryItems);
			builder.AddAttribute(
			    5,
			    "DepOnAddNew",
			    HandleCreateCategory
			);
			builder.CloseComponent();
		}
		,
		"read" or "delete" => builder =>

	{
		builder.OpenComponent(0, typeof(ModelReadDeleteForm));
		builder.AddAttribute(1, "Dummy2", Item);
		builder.AddAttribute(2, "Operation", operation);
		builder.CloseComponent();
	}
		,

		_ => builder =>
		{
			builder.AddMarkupContent(0, "<p class=\"alert alert-warning\">Invalid Operation</p>");
		}
	};

	private RenderFragment DepForm(string operation) => builder =>
	{
		builder.OpenComponent(0, typeof(DepModelCreateForm));
		builder.AddAttribute(1, "Category", CategoryItem);
		builder.AddAttribute(2, "Operation", operation);
		builder.AddAttribute(3, "EditContext", categoryEditContext);
		builder.CloseComponent();
	};

	// Creates an EditContext for any provided entity.
	private static EditContext CreateEditContext(Dummy2 item) => new EditContext(item);
	private static EditContext CategoryCreateEditContext(Category item) => new EditContext(item);

	// Helper methods used to tweak Modal title.
	// GetModalTitle depends on GetTitle.

	private string GetTitle(Dummy2 item) => item?.Name ?? "";
	private string GetModalTitle(string crudOper, Dummy2 item)
	{
		string OperTitle = crudOper switch
		{
			"create" => "Create New",
			"read" => "Details for",
			"update" => "Edit",
			"delete" => "Delete",
			_ => "Operation"
		};
		string itemName = "";
		// When creating a record, the item is null.
		// The following code retrieves the itemName only for already existing records.
		if (item != null)
		{
			itemName = GetTitle(item);
		}
		return string.IsNullOrWhiteSpace(itemName)
			? $"{OperTitle} Record"
			: $"{OperTitle} {itemName}";
	}

	// The pivotal method to be tweaked for shifting back and forth to dependent table .
	private async Task HandleSubmit(Dummy2 item, string? operation)
	{
		// [HCOM-DESIGN NOTE]
		// HandleSubmit() = central submission handler for Dummy2 modal.
		// - Executes CRUD operation-specific logic in a unified async context.
		// - Validates form input before performing any database change.
		// - Logs and handles exceptions gracefully in catch.
		// - Ensures consistent UI state by closing the modal and refreshing the item list.
		// This guarantees the Dummy page stays synchronized with backend state.
		// The flow starts by defining both ExecValidOperation and ExecInvalidOperation
		// then using try/catch/finally combo to fulfill the explained goals.

		async Task<bool> ExecValidOperation()
		{
			switch (operation)
			{
				case "create":
					context.Dummy2.Add(item);
					// FOR REMOVAL: Logging with successful in memory creation with valid form data.
					Logger.LogInformation("Item being created in memory not in database.");
					break;

				case "update":
					context.Dummy2.Update(item);
					// FOR REMOVAL: Logging with successful in memory updating with valid form data.
					Logger.LogInformation("Item being updated in memory not in database.");
					break;

				case "delete":
					context.Dummy2.Remove(item);
					// FOR REMOVAL: Logging with successful in memory removal with valid form data.
					Logger.LogInformation("Item being deleted in memory not in database.");
					break;

				case "read":
					break;
			}
			await context!.SaveChangesAsync();

			if (operation is "create" or "update" or "delete")
			{
				Logger.LogInformation($"Item successfully {operation}d.");
				await NotifySrvs.AddMessage(
					text: $"Item successfully {operation}d",
					lvl: MessageLevel.Success
				);
			}

			return true;
		}

		async Task<bool> ExecInvalidOperation()
		{
			Logger.LogInformation("Validation failed. Form will remain open.");
			await InvokeAsync(StateHasChanged);
			return false;
		}

		bool shouldClose = false;

		try
		{
			shouldClose = await (operation switch
			{
				"create" or "update" when editcontext!.Validate() => ExecValidOperation(),
				"create" or "update" => ExecInvalidOperation(),
				"delete" or "read" => ExecValidOperation(),
				_ => Task.FromResult(false)
			});
		}

		catch (DbUpdateConcurrencyException ex)
		{
			Logger.LogWarning(ex, "Concurrency conflict during database operation");
		}

		catch (DbUpdateException ex)
		{
			Logger.LogError(ex, "Database update failed");
		}

		catch (Exception ex)
		{
			Logger.LogError(ex, $"Unhandled exception during {operation} operation.");
		}

		finally
		{
			if (shouldClose)
			{
				await ModalHideAsync<Dummy2>(_dummy2ModalRef!);
				await LoadItemsAsync();
				// var lastMessage = NotifySrvs.Messages.Last();
				// var ToastId = $"toast{lastMessage.Id}";
				// await JS.InvokeVoidAsync("finalizeSubmit", "theModal", ToastId);
				Logger.LogInformation("Modal closed after successful operation.");
			}
			else
			{
				Logger.LogInformation("Modal remains open after failed operation or invalid form.");
			}
		}
	}

	private async Task HandleSubmitCategory(Category item, string? operation)
	{
		// [HCOM-DESIGN NOTE]
		// HandleSubmitCategory() = Create submission for dependent Category modal.
		// - Executes create operation logic in a unified async context.
		// - Validates the form input before performing any database change.
		// - Logs and handles exceptions gracefully in catch areas.
		// - Ensures consistent UI state by closing the dependent Modal, opening the primary Modal with refreshing the Dependent Entries in the Primary Modal.

		// This guarranties Category is being synced with the backend state.
		// The flow starts by defining both ExecValidOperation and ExecInvalidOperation
		// then using try/catch/finally combo to fulfill the explained goals.

		async Task<bool> ExecValidOperation()
		{
			// The same steps for HandleSubmit without the switch.
			// Note that the Dependent Modal is only concerned with create operation submission.
			context.Category.Add(item);
			Logger.LogInformation("Item being created in memory not in database.");
			await context!.SaveChangesAsync();
			Logger.LogInformation("Item successfully created.");
			await NotifySrvs.AddMessage(
			    text: "Item successfully created.",
			    lvl: MessageLevel.Success
			);

			return true;
		}

		// Identical to original HandleSubmit.
		async Task<bool> ExecInvalidOperation()
		{
			Logger.LogInformation("Validation failed. Form will remain open.");
			await InvokeAsync(StateHasChanged);
			return false;
		}

		bool shouldClose = false;
		try
		{
			shouldClose = await (operation switch
			{
				"create" when categoryEditContext!.Validate() => ExecValidOperation(),
				"create" => ExecInvalidOperation(),
				_ => Task.FromResult(false)
			});
		}

		catch (DbUpdateConcurrencyException ex)
		{
			Logger.LogWarning(ex, "Concurrency conflict during database operation");
		}


		catch (Exception ex)
		{
			Logger.LogError(ex, "Unhandled exception during create operation.");
		}
		finally
		{
			if (shouldClose)
			{
				await DepModalPostSubmissionHandler();
				Logger.LogInformation("Modal closed after successful operation.");
			}
			else
			{
				Logger.LogInformation("Modal remains open after failed operation or invalid form.");
			}
		}
	}
}
