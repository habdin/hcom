using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;

using blazor_hcom.Models;

using blazor_hcom.Classes;
using blazor_hcom.Components.Layout;

namespace blazor_hcom.Components.Pages.DummyPages;

public partial class Index : ComponentBase, IAsyncDisposable
{
	// Modal Related properties that used to tweak the <Modal />
	private Modal<Dummy>? _myModalRef;
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

	// Method that builds and renders the internal form of the modal
	private RenderFragment MainForm(string operation) => operation switch
	{
		"create" or "update" => builder =>
		{
			builder.OpenComponent(0, typeof(ModelCreateUpdateForm));
			builder.AddAttribute(1, "Dummy", Item);
			builder.AddAttribute(2, "Operation", operation);
			builder.AddAttribute(3, "EditContext", editcontext);
			builder.CloseComponent();
		}
		,
		"read" or "delete" => builder =>

	{
		builder.OpenComponent(0, typeof(ModelReadDeleteForm));
		builder.AddAttribute(1, "Dummy", Item);
		builder.AddAttribute(2, "Operation", operation);
		builder.CloseComponent();
	}
		,

		_ => builder =>
		{
			builder.AddMarkupContent(0, "<p class=\"alert alert-warning\">Invalid Operation</p>");
		}
	};

	// Creates an EditContext for any provided entity.
	private static EditContext CreateEditContext(Dummy item) => new EditContext(item);

	// Helper methods used to tweak Modal title.
	// GetModalTitle depends on GetTitle.
	private string GetTitle(Dummy item) => item?.Name ?? "";
	private string GetModalTitle(string crudOper, Dummy item)
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
		if (item != null)
		{
			itemName = GetTitle(item);
		}
		return string.IsNullOrWhiteSpace(itemName)
			? $"{OperTitle} Record"
			: $"{OperTitle} {itemName}";
	}
	private async Task HandleSubmit(Dummy item, string? operation)
	{
		// [HCOM-DESIGN NOTE]
		// HandleSubmit() = central submission handler for Dummy modal.
		// - Executes CRUD operation-specific logic in a unified async context.
		// - Validates form input before performing any database changes.
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
					context.Dummy.Add(item);
					// FOR REMOVAL: Logging with successful in memory creation with valid form data.
					Logger.LogInformation("Item being created in memory not in database.");
					break;

				case "update":
					context.Dummy.Update(item);
					// FOR REMOVAL: Logging with successful in memory updating with valid form data.
					Logger.LogInformation("Item being updated in memory not in database.");
					break;

				case "delete":
					context.Dummy.Remove(item);
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
				await ModalHideAsync<Dummy>(_myModalRef!);
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

}
