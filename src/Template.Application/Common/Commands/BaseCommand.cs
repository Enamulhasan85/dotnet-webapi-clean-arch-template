namespace Template.Application.Common.Commands;

public interface ICommand<TResult> { }

public abstract class BaseCommand<TResult> : ICommand<TResult>
{
    // standard base command properties
    public string? CreatedBy { get; set; }
    public Guid CorrelationId { get; set; } = Guid.NewGuid();

}
