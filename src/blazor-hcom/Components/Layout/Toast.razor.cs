using Microsoft.AspNetCore.Components;
using blazor_hcom.Classes;

namespace blazor_hcom.Components.Layout;


public partial class Toast : ComponentBase, IDisposable
{
    // ==============================
    // Properties and variables 
    // ==============================
    [Parameter] public AppMessage? Message { get; set; }
    [Parameter] public bool AutoHide { get; set; } = true;
    [Parameter] public int Delay { get; set; } = 5000;
    [Parameter] public Action<Guid, Toast>? Register { get; set; }
    [Parameter] public EventCallback<Guid> OnDeleted { get; set; }
    [Parameter] public EventCallback<Guid> OnHidden { get; set; }
    [Parameter] public DateTime CurrentTime { get; set; }

    private bool IsVisible { get; set; }
    private bool IsShown { get; set; }
    private string? ToastId => $"toast-{Message?.Id}";
    private CancellationTokenSource? _cts;
    private Guid? _lastMessageId;

    // ==============================
    // Methods
    // ==============================

    // protected override void OnAfterRender(bool firstRender)
    // {
    // 	if (firstRender) {
    //         Register?.Invoke(Message!.Id, this);
    //     }
    // }

    protected override async Task OnParametersSetAsync()
    {
        // Exit (no render) if the Message is null
        if (Message is null)
            return;

        // Exit (no render) if the Message is not new
        if (_lastMessageId == Message.Id)
            return;

        // This protects from re-render attempts for the same Message.
        _lastMessageId = Message.Id;
        Register?.Invoke(Message.Id, this);

        // Shows the Toast
        await Show();

        // Hides the Toast after a set period of Time.
        if (AutoHide)
            _ = AutoHideAsync();
    }

    private async Task AutoHideAsync()
    {
        // Use CancellationTokenSource to avoid race conditions:
        // Example a user clicks close button while hiding is in progress.
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        try
        {
            if (!AutoHide || Delay <= 0)
                return;

            await Task.Delay(Delay, _cts.Token);
            await Hide();
            await InvokeAsync(StateHasChanged);
        }
        catch (TaskCanceledException) { }
    }

    public async Task Show()
    {
        IsVisible = true;
        await InvokeAsync(StateHasChanged);

        await Task.Delay(150);
        IsShown = true;

        await InvokeAsync(StateHasChanged);
    }

    public async Task Hide()
    {
        _cts?.Cancel();
        IsShown = false;
        await InvokeAsync(StateHasChanged);

        await Task.Delay(150);
        IsVisible = false;

	// Initiate a re-render.
        await InvokeAsync(StateHasChanged);

	// Pushes up the message that is hidden to the parent component
        await MessageHidden(Message!.Id);
    }

    public async Task Refresh() => await InvokeAsync(StateHasChanged);

    public async Task MessageDeleted(Guid id) => await OnDeleted.InvokeAsync(id);

    public async Task MessageHidden(Guid id) => await OnHidden.InvokeAsync(id);

    public void Dispose()
    {
        // Get rid of Any CancellationTokenSource
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
