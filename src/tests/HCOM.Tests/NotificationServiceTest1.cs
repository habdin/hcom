using Xunit;
using blazor_hcom.Classes;
using blazor_hcom.Services;

namespace HCOM.Tests;

public class NotificationServiceTests
{
    // Declare the Message List
    private NotificationService _notifysrvs = new NotificationService();

    // Test that AddMessage adds a message to the list
    [Fact]
    public async Task ShouldAddMessage()
    {
        // The purpose of the test is to check if the Messages count
        // was incremented and that the new message text or MessageLevel
        // is the one that is Asserted for equality. The test is started
        // by asserting that originally the last message if any doesn't
        // include the message text or messagelevel.


        var LastMessage = _notifysrvs.Messages.LastOrDefault();
        if (LastMessage is not null)
        { Assert.NotEqual("New Test Message 1 added.", LastMessage.Text); }
        // Declare the old Messages count
        int oldMsgsCount = _notifysrvs.Messages.Count;
        // A service caller will call AddMessage with its correct signature
        await _notifysrvs.AddMessage(
            "New Test Message 1 added.", MessageLevel.Info
        );
        // Declare the new Messages Count
        int newMsgsCount = _notifysrvs.Messages.Count;
        var newLastMessage = _notifysrvs.Messages.Last();

        Assert.Equal("New Test Message 1 added.", newLastMessage.Text);
        Assert.Equal(MessageLevel.Info, newLastMessage.Level);
        Assert.Equal(oldMsgsCount + 1, newMsgsCount);
    }

    // Test that RemoveMessage removes a specific message
    [Fact]
    public async Task ShouldRemoveMessage()
    {
        await _notifysrvs.AddMessage("Message for removal.", MessageLevel.Info);
        var messageForRemoval = _notifysrvs.Messages.Last();
        Guid idForMessageForRemoval = messageForRemoval.Id;
        Assert.Equal("Message for removal.", messageForRemoval.Text);
        Assert.Equal(MessageLevel.Info, messageForRemoval.Level);
        await _notifysrvs.RemoveMessage(idForMessageForRemoval);
        Assert.DoesNotContain(
            _notifysrvs.Messages, m => m.Id == idForMessageForRemoval
        );
    }

    // Test that Clear removes all messages
    [Fact]
    public async Task ShouldClearAllMessages()
    {

        // Arrange messages
        await _notifysrvs.AddMessage("Message 1 for removal.", MessageLevel.Info);
        await _notifysrvs.AddMessage("Message 2 for removal.", MessageLevel.Info);
        await _notifysrvs.AddMessage("Message 3 for removal.", MessageLevel.Info);
        // precondition assertions
        Assert.Equal(3, _notifysrvs.Messages.Count);
        // Perform action
        await _notifysrvs.ClearAllMessages();
        Assert.Empty(_notifysrvs.Messages);
    }

    // Test removing a message that doesn't exist will throw a
    // KeyNotFoundException as per service method definition.
    [Fact]
    public async Task RemoveMessage_WithNonExistentId_ThrowsKeyNotFoundException()
    {
        // Arrange: create a Guid that is not in the service
        Guid nonExistentId = Guid.NewGuid();

        // Act & Assert: removing it should throw
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _notifysrvs.RemoveMessage(nonExistentId));
    }

}
