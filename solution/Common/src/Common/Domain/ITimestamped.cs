namespace Common.Domain;

public interface ITimestamped
{
    /// <summary>
    /// When the entity was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// When the entity was modified.
    /// </summary>
    public DateTime ModifiedAt { get; }

    /// <summary>
    /// Who modified the entity.
    /// </summary>
    public Guid ModifiedBy { get; set; }
}