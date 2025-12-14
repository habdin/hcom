namespace blazor_hcom.Helpers;

public static class EntityReflection
{
	/// <summary>
    /// Get the integer value of the Id property of a Model or throw an Exception.
    /// </summary>
	public static int GetIdValue<TEntity>(TEntity entity) =>
		typeof(TEntity).GetProperties()
			.FirstOrDefault(p => p.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
		switch
        {
            null => throw new InvalidOperationException(
				$"'{typeof(TEntity).Name}' doesn't contain a property named 'Id'."),
			var prop => prop.GetValue(entity) switch
            {
                int id => id,
                null => throw new NullReferenceException($"'{typeof(TEntity).Name}.Id' has no value."),
                _ => throw new InvalidCastException($"'{typeof(TEntity).Name}.Id' is not of type int.")
            }
        };
}
