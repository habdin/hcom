using blazor_hcom.Classes;

namespace blazor_hcom.Services;

public interface IMessageService
{
	IReadOnlyList<AppMessage> Messages { get; }

    Task AddMessage(string text, MessageLevel lvl=MessageLevel.Info);
    Task RemoveMessage(Guid id);
    Task ClearAllMessages();

    event Func<AppMessage, Task>? OnMessageAdded;
    event Func<Task>? OnMessagesChanged;
}
