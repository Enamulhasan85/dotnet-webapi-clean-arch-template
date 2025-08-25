namespace Template.Application.Common.Queries;

public interface IQuery<TResult> { }

public abstract class BaseQuery<TResult> : IQuery<TResult>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
