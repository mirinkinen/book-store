namespace Common.Application;

/// <summary>
/// Wraps IQueryable objects to disable Wolverine's cascade logic.
/// </summary>
public record QueryableResponse<T>(IQueryable<T> Query) where T : class;