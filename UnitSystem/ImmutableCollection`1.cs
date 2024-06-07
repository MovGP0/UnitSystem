using System.Collections.ObjectModel;
using UnitSystem.Extensions;

namespace UnitSystem;

public class ImmutableCollection<TItem> : ReadOnlyCollection<TItem>, IEquatable<ImmutableCollection<TItem>>
{
    private readonly int _hashCode;

    public ImmutableCollection(IList<TItem> items)
        : base(items)
    {
        _hashCode = Items.Hash();
    }

    public bool Equals(ImmutableCollection<TItem>? other)
    {
        if (other == null)
        {
            return false;
        }

        return this.SequenceEqual(other);
    }

    public override bool Equals(object? obj) => obj is ImmutableCollection<TItem> collection && Equals(collection);

    public override int GetHashCode() => _hashCode;
}
