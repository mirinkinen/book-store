namespace Common.Application;

/// <summary>
/// Wrapper data structure to contain IEnumerable ja IQueryable objects.
/// This disables the Wolverine's cascading messages logic.
/// </summary>
public record QueryableResponse<T>(IQueryable<T> Query) where T : class;