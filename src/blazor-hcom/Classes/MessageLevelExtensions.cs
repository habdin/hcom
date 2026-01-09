namespace blazor_hcom.Classes;

public static class MessageLevelExtensions
{
    public static string ToBootStrapColor(this MessageLevel level) => level switch
    {
        MessageLevel.Info => "info",
        MessageLevel.Success => "success",
        MessageLevel.Warning => "warning",
        MessageLevel.Error => "danger",
        _ => "secondary"
    };
}
