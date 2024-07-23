namespace QuantitySystem.Units;

public partial class Unit : IEquatable<Unit>
{
    /// <inheritdoc cref="IEquatable{T}.Equals(object?)"/>
    public override bool Equals(object? obj)
        => obj is Unit unit && Equals(unit);

    /// <inheritdoc cref="IEquatable{T}.Equals(T)"/>
    public bool Equals(Unit? other)
        => other is not null && Symbol.Equals(other.Symbol, StringComparison.Ordinal);

    /// <inheritdoc cref="object.GetHashCode()"/>
    public override int GetHashCode() => Symbol.GetHashCode();

    public static bool operator ==(Unit? lhs, Unit? rhs)
    {
        return ReferenceEquals(lhs, rhs)
               || lhs is not null && rhs is not null && lhs.Equals(rhs);
    }

    public static bool operator !=(Unit lhs, Unit rhs) => !(lhs == rhs);
}