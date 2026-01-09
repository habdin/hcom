using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace blazor_hcom.Components.Layout;

public partial class Modal<TEntity> : ComponentBase
{
    // Variables and properties are coded as the order they appear
    // within the razor itself. Same applies for methods.
    // This eases coding.

	// =========================================
    // Properties and fields
	// =========================================
    [Parameter] public TEntity? Item { get; set; }
    [Parameter] public bool Fading { get; set; }
    private ElementReference _modalRoot;

    // Used to toggle the visibility of certain classes in the Modal
    public bool IsVisible { get; private set; }

    // Since the IsVisible is a Modal local property, there should be
    // another mechanism to have the same effect on the body element of
    // the whole document. This is only possible via a eventcallback<bool>
    // that catches that the modal was opened so that this is reflected
    // in the body tag in App.razor via a conditional.
    private bool IsShown;

    [Parameter]
	public EventCallback<bool> OnVisibilityChanged { get; set; }

    // The Operation and Title parameters are needed to
    // properly control the modal's title within Parent component.
    [Parameter] public string? Operation { get; set; }
    [Parameter] public string? Title { get; set; }
	// Used to close the modal (Responce to user btn click events)
    [Parameter] public EventCallback OnClose { get; set; }
	[Parameter] public EventCallback BackDropClick { get; set; }

    // Setting the enum for ModalStage and using it.
    // The ModalStage lets the Modal be used with Interdependent
    // database entities.
    public enum ModalStage
	{
		MainForm,
		DependentForm
	}
	[Parameter] public ModalStage Stage { get; set; }

	// Setting an Enum for ModalSize and use it.
	public enum ModalSize
	{
		Small,
		Default,
		Large,
		ExtraLarge
	}
	[Parameter] public ModalSize Size { get; set; }
	private string ModalSizeClass => Size switch
	{
		ModalSize.Small => "modal-sm",
		ModalSize.Large => "modal-lg",
		ModalSize.ExtraLarge => "modal-xl",
		_ => ""
	};
	// Setting an Enum for Backdrop and use it.
	[Parameter]
	public ModalBackdrop Backdrop { get; set; }
	public enum ModalBackdrop
	{
		Enabled,
		Disabled,
		Static
	}
    private string BackdropClass => Backdrop switch
    {
        ModalBackdrop.Enabled => "modal-backdrop show fade",
        ModalBackdrop.Static => "modal-backdrop show fade",
        ModalBackdrop.Disabled => string.Empty,
        _ => string.Empty
    };

    [Parameter] public RenderFragment? MainFormContent { get; set; }
	[Parameter] public RenderFragment? DependentFormContent { get; set; }
	// Used to customize the Text of the Submit button.
	// Similar to Modal title, it is affected by the crud operation.
	[Parameter] public string? BtnSubmitText { get; set; }
	// The inner form's submit btn is totally removed and its behaviour
	// is totally passed to the modal. This event is used to respond
	// to modal submit btn click.
	[Parameter] public EventCallback OnSubmit { get; set; }
	//=============================================================
	
    // ========================================
    // Methods
    // ========================================
	
	private async Task HandleKeyDown (KeyboardEventArgs e)
	{
		if (e.Key == "Escape")
		{
            await Hide();
        }
	}

	private async Task HandleBackdropClicked() {
		if (Backdrop != ModalBackdrop.Static)
		{
			await BackDropClick.InvokeAsync();
		}
	}

    public async Task Show()
	{
        IsVisible = true;
		await InvokeAsync(StateHasChanged);
		// Allow modal animation to take place
        await Task.Delay(150);
        IsShown = true;

		// When the modal appears it gets the Focus.
        await _modalRoot.FocusAsync();

        // Allow app-root tag changes to occur
        await OnVisibilityChanged.InvokeAsync(true);
		await InvokeAsync(StateHasChanged);
    }

    public async Task Hide()
    {
		// Just swap the order of appearance of IsShown and IsVisible
		// that were present in Show and set them both to false
		// to do hide animations
        IsShown = false;
        await InvokeAsync(StateHasChanged);

		await Task.Delay(150);
		IsVisible = false;
		// Allow app-root changes to be reverted back to normal.
		await OnVisibilityChanged.InvokeAsync(false);
        await InvokeAsync(StateHasChanged);
    }
}
