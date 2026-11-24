using ShInUeXx.Numerics;

namespace UnitSystem.Gauge;

/// <summary>
/// An irreducible representation of the Standard Model's color, weak isospin, and hypercharge factors.
/// </summary>
public sealed record GaugeRepresentation
{
    /// <summary>
    /// Creates a gauge representation. Weak isospin is supplied as twice its value.
    /// Hypercharge uses the convention that electric charge is weak isospin's third component plus hypercharge.
    /// </summary>
    public GaugeRepresentation(ColorRepresentation color, uint twiceWeakIsospin, Rational hypercharge)
    {
        if (!Rational.IsFinite(hypercharge))
        {
            throw new ArgumentOutOfRangeException(nameof(hypercharge), "Hypercharge must be finite.");
        }

        Color = color;
        TwiceWeakIsospin = twiceWeakIsospin;
        Hypercharge = hypercharge;
    }

    /// <summary>Gets the color representation.</summary>
    public ColorRepresentation Color { get; }

    /// <summary>Gets twice the weak isospin. Zero is a singlet and one is a doublet.</summary>
    public uint TwiceWeakIsospin { get; }

    /// <summary>Gets the exact weak hypercharge.</summary>
    public Rational Hypercharge { get; }

    /// <summary>Gets the trivial representation under all three gauge factors.</summary>
    public static GaugeRepresentation Singlet { get; } = new(ColorRepresentation.Singlet, 0, Rational.Zero);

    /// <summary>Gets the conjugate representation. Weak SU(2) representations are self-conjugate.</summary>
    public GaugeRepresentation Conjugate() => new(Color.Conjugate(), TwiceWeakIsospin, -Hypercharge);
}
