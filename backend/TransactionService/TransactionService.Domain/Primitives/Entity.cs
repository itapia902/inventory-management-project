namespace TransactionService.Domain.Primitives;


public abstract class Entity<TId> : IEquatable<Entity<TId>>
{
    public TId Id { get; private init; }

    protected Entity(TId id)
    {
        Id = id;
    }

    public bool Equals(Entity<TId>? other) => Equals((object?)other);
    public override bool Equals(object? obj) => obj is Entity<TId> entity && Id!.Equals(entity.Id);

    public static bool operator ==(Entity<TId> left, Entity<TId> right) => Equals(left, right);

    public static bool operator !=(Entity<TId> left, Entity<TId> right) => !Equals(left, right);

    public override int GetHashCode() => Id!.GetHashCode();
}