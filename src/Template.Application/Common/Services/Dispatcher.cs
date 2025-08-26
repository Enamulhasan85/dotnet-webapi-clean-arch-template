using Microsoft.Extensions.DependencyInjection;
using Template.Application.Common.Commands;
using Template.Application.Common.Interfaces;
using Template.Application.Common.Queries;

namespace Template.Application.Common.Services
{
    /// <summary>
    /// Strongly-typed dispatcher implementation without reflection
    /// </summary>
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Send a command to its handler with compile-time type safety
        /// </summary>
        /// <typeparam name="TCommand">The command type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="command">The command to send</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result</returns>
        public async Task<TResult> Send<TCommand, TResult>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : class
        {
            var handler = _serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
            return await handler.HandleAsync(command, cancellationToken);
        }

        /// <summary>
        /// Send a query to its handler with compile-time type safety
        /// </summary>
        /// <typeparam name="TQuery">The query type</typeparam>
        /// <typeparam name="TResult">The result type</typeparam>
        /// <param name="query">The query to send</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The result</returns>
        public async Task<TResult> Query<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : class
        {
            var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
            return await handler.HandleAsync(query, cancellationToken);
        }
    }
}