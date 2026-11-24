namespace UnitSystem.Gauge;

/// <summary>
/// An irreducible representation of color SU(3), identified by its Dynkin labels.
/// </summary>
public readonly record struct ColorRepresentation
{
    /// <summary>Creates a representation from its two nonnegative Dynkin labels.</summary>
    public ColorRepresentation(uint p, uint q)
    {
        P = p;
        Q = q;
    }

    /// <summary>Gets the first Dynkin label.</summary>
    public uint P { get; }

    /// <summary>Gets the second Dynkin label.</summary>
    public uint Q { get; }

    /// <summary>Gets the trivial representation.</summary>
    public static ColorRepresentation Singlet => default;

    /// <summary>Gets the fundamental representation.</summary>
    public static ColorRepresentation Triplet => new(1, 0);

    /// <summary>Gets the conjugate of the fundamental representation.</summary>
    public static ColorRepresentation Antitriplet => new(0, 1);

    /// <summary>Gets the adjoint representation.</summary>
    public static ColorRepresentation Octet => new(1, 1);

    /// <summary>Gets the complex-conjugate representation.</summary>
    public ColorRepresentation Conjugate() => new(Q, P);
}
