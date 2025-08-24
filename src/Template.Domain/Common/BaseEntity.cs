namespace Template.Domain.Common;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; protected set; } = default!;
}

public abstract class AuditableEntity<TKey> : BaseEntity<TKey>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
    public string? LastModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public void MarkDeleted(string? userId = null)
    {
        IsDeleted = true;
        LastModifiedAt = DateTime.UtcNow;
        LastModifiedBy = userId;
    }
}

