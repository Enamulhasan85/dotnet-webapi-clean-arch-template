using Template.Application.Common.Commands;
using Template.Application.Common.Queries;

namespace Template.Application.Common.Interfaces;

/// <summary>
/// Strongly-typed dispatcher interface for commands and queries
/// </summary>
public interface IDispatcher
{
    /// <summary>
    /// Send a command to its handler with strong typing
    /// </summary>
    /// <typeparam name="TCommand">The command type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="command">The command to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result</returns>
    Task<TResult> Send<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : class;

    /// <summary>
    /// Send a query to its handler with strong typing
    /// </summary>
    /// <typeparam name="TQuery">The query type</typeparam>
    /// <typeparam name="TResult">The result type</typeparam>
    /// <param name="query">The query to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result</returns>
    Task<TResult> Query<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : class;
}
