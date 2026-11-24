using ShInUeXx.Numerics;

namespace UnitSystem.Gauge;

/// <summary>
/// A built-in gauge multiplet for the Higgs or a fermion of one Standard Model generation.
/// </summary>
public union StandardModelGaugeRepresentations(
    StandardModelGaugeRepresentations.LeftHandedQuarkDoublet,
    StandardModelGaugeRepresentations.RightHandedUpQuark,
    StandardModelGaugeRepresentations.RightHandedDownQuark,
    StandardModelGaugeRepresentations.LeftHandedLeptonDoublet,
    StandardModelGaugeRepresentations.RightHandedChargedLepton,
    StandardModelGaugeRepresentations.Higgs)
{
    /// <summary>The left-handed quark doublet.</summary>
    public sealed record LeftHandedQuarkDoublet;

    /// <summary>The right-handed up-type quark multiplet.</summary>
    public sealed record RightHandedUpQuark;

    /// <summary>The right-handed down-type quark multiplet.</summary>
    public sealed record RightHandedDownQuark;

    /// <summary>The left-handed lepton doublet.</summary>
    public sealed record LeftHandedLeptonDoublet;

    /// <summary>The right-handed charged lepton multiplet.</summary>
    public sealed record RightHandedChargedLepton;

    /// <summary>The Higgs doublet.</summary>
    public sealed record Higgs;

    /// <summary>Gets the gauge representation of this multiplet.</summary>
    /// <exception cref="InvalidOperationException">The union has no active multiplet.</exception>
    public readonly GaugeRepresentation Representation => this switch
    {
        LeftHandedQuarkDoublet => QuarkDoublet,
        RightHandedUpQuark => UpQuark,
        RightHandedDownQuark => DownQuark,
        LeftHandedLeptonDoublet => LeptonDoublet,
        RightHandedChargedLepton => ChargedLepton,
        Higgs => HiggsDoublet,
        null => throw new InvalidOperationException("A Standard Model multiplet must be selected."),
    };

    /// <summary>Gets the left-handed quark doublet.</summary>
    public static GaugeRepresentation QuarkDoublet { get; } = new(ColorRepresentation.Triplet, 1, new Rational(1, 6));

    /// <summary>Gets the right-handed up-type quark representation.</summary>
    public static GaugeRepresentation UpQuark { get; } = new(ColorRepresentation.Triplet, 0, new Rational(2, 3));

    /// <summary>Gets the right-handed down-type quark representation.</summary>
    public static GaugeRepresentation DownQuark { get; } = new(ColorRepresentation.Triplet, 0, new Rational(-1, 3));

    /// <summary>Gets the left-handed lepton doublet.</summary>
    public static GaugeRepresentation LeptonDoublet { get; } = new(ColorRepresentation.Singlet, 1, new Rational(-1, 2));

    /// <summary>Gets the right-handed charged lepton representation.</summary>
    public static GaugeRepresentation ChargedLepton { get; } = new(ColorRepresentation.Singlet, 0, -Rational.One);

    /// <summary>Gets the Higgs doublet.</summary>
    public static GaugeRepresentation HiggsDoublet { get; } = new(ColorRepresentation.Singlet, 1, new Rational(1, 2));
}
