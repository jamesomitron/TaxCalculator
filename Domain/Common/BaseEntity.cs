namespace Domain.Common;

public abstract record BaseEntity
{
    public Guid Id { get; init; }

    public DateTime DateCreated { get; init; }
}
