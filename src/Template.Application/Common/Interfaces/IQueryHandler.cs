namespace Template.Application.Common.Interfaces;

/// <summary>
/// Handler interface for queries with return values
/// </summary>
/// <typeparam name="TQuery">The query type</typeparam>
/// <typeparam name="TResult">The result type</typeparam>
public interface IQueryHandler<in TQuery, TResult>
    where TQuery : class
{
    /// <summary>
    /// Handle the query and return a result
    /// </summary>
    /// <param name="query">The query to handle</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result</returns>
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
