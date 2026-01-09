namespace blazor_hcom.Classes;

public record AppMessage
(
	Guid Id,
	string Text,
	MessageLevel Level = MessageLevel.Info,
	DateTime CreatedAt = default
);
