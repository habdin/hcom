namespace blazor_hcom.Services;

public class UiStateService
{
	private bool _modalOpen;
	public bool ModalOpen
	{
		get => _modalOpen;
		set
		{
			if (_modalOpen == value) return;
			_modalOpen = value;
			OnChange?.Invoke();
		}
	}

	public event Action? OnChange;
}
