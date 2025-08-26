namespace Template.Application.Common.Interfaces;

/// <summary>
/// Handler interface for commands with return values
/// </summary>
/// <typeparam name="TCommand">The command type</typeparam>
/// <typeparam name="TResult">The result type</typeparam>
public interface ICommandHandler<in TCommand, TResult>
    where TCommand : class
{
    /// <summary>
    /// Handle the command and return a result
    /// </summary>
    /// <param name="command">The command to handle</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result</returns>
    Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
