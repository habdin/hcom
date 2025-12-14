using blazor_hcom.Classes;

namespace blazor_hcom.Services;

public class NotificationService : IMessageService
{
	private readonly object _lock = new();
	private readonly List<AppMessage> _messages = new();
    public IReadOnlyList<AppMessage> Messages => _messages.AsReadOnly<AppMessage>();

	public async Task AddMessage(string text, MessageLevel lvl = MessageLevel.Info)
	{
        var msg = new AppMessage(
            Id: Guid.NewGuid(),
            Text: text,
            Level: lvl,
            CreatedAt: DateTime.UtcNow
        );
		lock (_lock)
		{
            _messages.Add(msg);
        }
        var msg_added_handler = OnMessageAdded;
		if (msg_added_handler is not null)
            await msg_added_handler.Invoke(msg);

        var handler = OnMessagesChanged;
		if (handler is not null)
            await handler.Invoke();
    }
	
	public async Task RemoveMessage(Guid id)
	{
		lock(_lock) {
            var msg = _messages.FirstOrDefault(m => m.Id == id)
                ?? throw new KeyNotFoundException($"Message with Id {id} not found.");
            _messages.Remove(msg);
        }
        var handler = OnMessagesChanged;
		if (handler is not null)
            await handler.Invoke();
	}
	
	public async Task ClearAllMessages () {
		lock(_lock)
		{
			if (_messages.Count == 0) return;
            _messages.Clear();
        }
        var handler = OnMessagesChanged;
		if (handler is not null)
            await handler.Invoke();
	}

    public event Func<AppMessage, Task>? OnMessageAdded;
    public event Func<Task>? OnMessagesChanged;
}
