using ShInUeXx.Numerics;
using UnitSystem.Dimensions;

namespace UnitSystem;

/// <summary>
/// Represents a unit within a specific unit system, defined by its dimension and factor.
/// </summary>
public abstract partial class Unit : IEquatable<Unit>
{
    private readonly int _hashCode;

    /// <summary>
    /// Initializes a new instance of the <see cref="Unit"/> class.
    /// </summary>
    /// <param name="system">The unit system to which the unit belongs.</param>
    /// <param name="factor">The factor by which the unit is multiplied.</param>
    /// <param name="offset">The offset of the unit from zero.</param>
    /// <param name="dimension">The dimension of the unit.</param>
    internal Unit(IUnitSystem system, double factor, double offset, Dimension dimension)
    {
        Check.Argument(dimension, nameof(dimension)).IsNotNull();
        if (factor == 0 || double.IsNaN(factor) || double.IsInfinity(factor))
        {
            throw new ArgumentOutOfRangeException(nameof(factor), "A unit requires a finite, nonzero scale factor.");
        }

        if (double.IsNaN(offset) || double.IsInfinity(offset))
        {
            throw new ArgumentOutOfRangeException(nameof(offset), "A unit requires a finite offset.");
        }

        System = system;
        Dimension = dimension;
        Factor = factor;
        Offset = offset;

        _hashCode = GenerateHashCode();
    }

    /// <summary>
    /// Gets the unit system to which the unit belongs.
    /// </summary>
    [Pure]
    internal IUnitSystem System { get; }

    /// <summary>
    /// Gets the factor by which the unit is multiplied.
    /// </summary>
    [Pure]
    internal double Factor { get; }

    /// <summary>
    /// Gets the offset of the unit from zero.
    /// </summary>
    /// <remarks>
    /// This offset is added after scaling a point into the coherent unit.
    /// </remarks>
    [Pure]
    internal double Offset { get; } = 0d;

    [Pure]
    public bool IsAbsoluteUnit => Offset == 0.0;

    internal bool SupportsPoints => !IsAbsoluteUnit || System is UnitSystem system && system.IsAffineDimension(Dimension);

    internal double ToCoherent(double amount, bool isPoint)
    {
        var scaled = amount * Factor;
        return isPoint && !IsAbsoluteUnit ? scaled + Offset : scaled;
    }

    internal double FromCoherent(double amount, bool isPoint)
    {
        var shifted = isPoint && !IsAbsoluteUnit ? amount - Offset : amount;
        return shifted / Factor;
    }

    /// <summary>
    /// Gets the dimension of the unit.
    /// </summary>
    [Pure]
    public Dimension Dimension { get; }

    /// <summary>
    /// Gets a value indicating whether the unit is coherent (i.e., its factor is 1).
    /// </summary>
    [Pure]
    internal bool IsCoherent => Factor.Equals(1.0) && Offset.Equals(0.0);

    /// <summary>
    /// Determines whether the specified unit is equal to the current unit.
    /// </summary>
    /// <param name="other">The unit to compare with the current unit.</param>
    /// <returns>true if the specified unit is equal to the current unit; otherwise, false.</returns>
    [Pure]
    public bool Equals(Unit? other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;

        return HasSameDimension(other)
               && Offset.Equals(other.Offset)
               && Factor.Equals(other.Factor);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current unit.
    /// </summary>
    /// <param name="obj">The object to compare with the current unit.</param>
    /// <returns>true if the specified object is equal to the current unit; otherwise, false.</returns>
    [Pure]
    public override bool Equals(object? obj) => obj is Unit unit && Equals(unit);

    /// <summary>
    /// Returns the hash code for the current unit.
    /// </summary>
    /// <returns>A hash code for the current unit.</returns>
    [Pure]
    public override int GetHashCode() => _hashCode;

    /// <summary>
    /// Determines whether two units belong to the same unit system.
    /// </summary>
    /// <param name="other">The unit to compare with the current unit.</param>
    /// <returns>true if the units belong to the same unit system; otherwise, false.</returns>
    [Pure]
    public bool HasSameSystem(Unit other) => System == other.System;

    /// <summary>
    /// Determines whether two units have the same dimension.
    /// </summary>
    /// <param name="other">The unit to compare with the current unit.</param>
    /// <returns>true if the units have the same dimension; otherwise, false.</returns>
    [Pure]
    public bool HasSameDimension(Unit other) => HasSameSystem(other) && Dimension.Equals(other.Dimension);

    /// <summary>
    /// Multiplies two units, resulting in a new unit with combined dimensions and factors.
    /// </summary>
    /// <param name="unit1">The first unit.</param>
    /// <param name="unit2">The second unit.</param>
    /// <returns>A new unit resulting from the multiplication.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="unit1"/> or <paramref name="unit2"/> is null.</exception>
    [Pure]
    public static Unit operator *(Unit unit1, Unit unit2)
    {
        Check.Argument(unit1, nameof(unit1)).IsNotNull();
        Check.Argument(unit2, nameof(unit2)).IsNotNull();
        Check.UnitsAreFromSameSystem(unit1, unit2);
        Check.EnsureAbsoluteUnit(unit1);
        Check.EnsureAbsoluteUnit(unit2);

        return unit1.System.CreateUnit(unit1.Factor * unit2.Factor, unit1.Dimension * unit2.Dimension);
    }

    /// <summary>
    /// Divides two units, resulting in a new unit with combined dimensions and factors.
    /// </summary>
    /// <param name="unit1">The first unit.</param>
    /// <param name="unit2">The second unit.</param>
    /// <returns>A new unit resulting from the division.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="unit1"/> or <paramref name="unit2"/> is null.</exception>
    [Pure]
    public static Unit operator /(Unit unit1, Unit unit2)
    {
        Check.Argument(unit1, nameof(unit1)).IsNotNull();
        Check.Argument(unit2, nameof(unit2)).IsNotNull();
        Check.UnitsAreFromSameSystem(unit1, unit2);
        Check.EnsureAbsoluteUnit(unit1);
        Check.EnsureAbsoluteUnit(unit2);

        var newFactor = unit1.Factor / unit2.Factor;
        var newDimension = unit1.Dimension / unit2.Dimension;
        return unit1.System.CreateUnit(newFactor, newDimension);
    }

    /// <summary>
    /// Raises a unit to a given exponent, resulting in a new unit with scaled dimension and factor.
    /// </summary>
    /// <param name="exponent">The exponent to raise this unit to.</param>
    /// <returns>A new unit resulting from the exponentiation.</returns>
    [Pure]
    public Unit Pow(int exponent) => Pow((Rational)exponent);

    /// <summary>
    /// Raises the unit to the round-trip decimal representation of a single-precision exponent.
    /// </summary>
    [Pure]
    public Unit Pow(float exponent) => Pow(DimensionExponent.FromSingle(exponent));

    /// <summary>
    /// Raises the unit to a rational power. The dimension is exact and the scale uses floating-point arithmetic.
    /// </summary>
    [Pure]
    public Unit Pow(Rational exponent)
    {
        Check.EnsureAbsoluteUnit(this);
        var dimension = Dimension.Pow(exponent);
        return System.CreateUnit(Math.Pow(Factor, exponent.ToDouble()), dimension);
    }

    /// <summary>
    /// Multiplies a unit by a scalar factor, resulting in a new unit with a scaled factor.
    /// </summary>
    /// <param name="unit">The unit to multiply.</param>
    /// <param name="factor">The scalar factor to multiply by.</param>
    /// <returns>A new unit resulting from the multiplication.</returns>
    [Pure]
    public static Unit operator *(Unit unit, double factor)
    {
        Check.EnsureAbsoluteUnit(unit);
        return unit.System.CreateUnit(factor * unit.Factor, unit.Dimension);
    }

    /// <summary>
    /// Multiplies a scalar factor by a unit, resulting in a new unit with a scaled factor.
    /// </summary>
    /// <param name="factor">The scalar factor to multiply by.</param>
    /// <param name="unit">The unit to multiply.</param>
    /// <returns>A new unit resulting from the multiplication.</returns>
    [Pure]
    public static Unit operator *(double factor, Unit unit)
    {
        Check.EnsureAbsoluteUnit(unit);
        return unit.System.CreateUnit(factor * unit.Factor, unit.Dimension);
    }

    /// <summary>
    /// Multiplies a prefix by a unit, resulting in a new unit with a scaled factor.
    /// </summary>
    /// <param name="prefix">The unit prefix to multiply by.</param>
    /// <param name="unit">The unit to multiply.</param>
    /// <returns>A new unit resulting from the multiplication.</returns>
    [Pure]
    public static Unit operator *(Unit unit, IUnitPrefix prefix)
    {
        Check.EnsureAbsoluteUnit(unit);
        return unit.System.CreateUnit(prefix.Factor * unit.Factor, unit.Dimension);
    }

    /// <summary>
    /// Multiplies a prefix by a unit, resulting in a new unit with a scaled factor.
    /// </summary>
    /// <param name="prefix">The unit prefix to multiply by.</param>
    /// <param name="unit">The unit to multiply.</param>
    /// <returns>A new unit resulting from the multiplication.</returns>
    [Pure]
    public static Unit operator *(IUnitPrefix prefix, Unit unit)
    {
        Check.EnsureAbsoluteUnit(unit);
        return unit.System.CreateUnit(prefix.Factor * unit.Factor, unit.Dimension);
    }

    /// <summary>
    /// Divides a unit by a scalar factor, resulting in a new unit with a scaled factor.
    /// </summary>
    /// <param name="unit">The unit to divide.</param>
    /// <param name="factor">The scalar factor to divide by.</param>
    /// <returns>A new unit resulting from the division.</returns>
    [Pure]
    public static Unit operator /(Unit unit, double factor)
    {
        Check.EnsureAbsoluteUnit(unit);
        return unit.System.CreateUnit(unit.Factor / factor, unit.Dimension);
    }

    /// <summary>
    /// Returns a string representation of the unit.
    /// </summary>
    /// <returns>A string representation of the unit.</returns>
    [Pure]
    public override string ToString() => System.Display(this);

    /// <summary>
    /// Generates a hash code for the unit based on its dimension and factor.
    /// </summary>
    /// <returns>A hash code for the unit.</returns>
    [Pure]
    private int GenerateHashCode() => HashCode.Combine(Dimension, Factor, Offset);

    /// <summary>
    /// Determines whether two units are equal.
    /// </summary>
    /// <param name="unit1">The first unit.</param>
    /// <param name="unit2">The second unit.</param>
    /// <returns>true if the units are equal; otherwise, false.</returns>
    [Pure]
    public static bool operator ==(Unit? unit1, Unit? unit2)
    {
        if (ReferenceEquals(unit1, unit2)) return true;
        if (unit1 is null || unit2 is null) return false;

        return unit1.HasSameDimension(unit2)
               && unit1.Factor.Equals(unit2.Factor)
               && unit1.Offset.Equals(unit2.Offset);
    }

    /// <summary>
    /// Determines whether two units are not equal.
    /// </summary>
    /// <param name="unit1">The first unit.</param>
    /// <param name="unit2">The second unit.</param>
    /// <returns>true if the units are not equal; otherwise, false.</returns>
    [Pure]
    public static bool operator !=(Unit unit1, Unit unit2) => !(unit1 == unit2);
}
