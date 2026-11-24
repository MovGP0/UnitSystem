namespace UnitSystem.Orientations;

/// <summary>A complex linear combination of the four Siano basis elements.</summary>
/// <param name="XCoefficient">The coefficient of the X basis element.</param>
/// <param name="YCoefficient">The coefficient of the Y basis element.</param>
/// <param name="ZCoefficient">The coefficient of the Z basis element.</param>
/// <param name="ScalarCoefficient">The coefficient of the orientationless identity.</param>
public readonly record struct SianoOrientation(
    Complex XCoefficient,
    Complex YCoefficient,
    Complex ZCoefficient,
    Complex ScalarCoefficient)
{
    /// <summary>The zero linear combination.</summary>
    public static SianoOrientation Zero => default;

    /// <summary>The orientationless identity.</summary>
    public static SianoOrientation Orientationless => new(0, 0, 0, 1);

    /// <summary>The first axis basis element.</summary>
    public static SianoOrientation X => new(1, 0, 0, 0);

    /// <summary>The second axis basis element.</summary>
    public static SianoOrientation Y => new(0, 1, 0, 0);

    /// <summary>The third axis basis element.</summary>
    public static SianoOrientation Z => new(0, 0, 1, 0);

    /// <summary>Gets whether this value is one of the four basis elements.</summary>
    public bool IsBasisElement => this == X || this == Y || this == Z || this == Orientationless;

    /// <summary>Adds the coefficients of corresponding basis elements.</summary>
    public static SianoOrientation operator +(SianoOrientation left, SianoOrientation right)
        => new(
            left.XCoefficient + right.XCoefficient,
            left.YCoefficient + right.YCoefficient,
            left.ZCoefficient + right.ZCoefficient,
            left.ScalarCoefficient + right.ScalarCoefficient);

    /// <summary>Scales all coefficients by the given value.</summary>
    public static SianoOrientation operator *(Complex factor, SianoOrientation orientation)
        => new(
            factor * orientation.XCoefficient,
            factor * orientation.YCoefficient,
            factor * orientation.ZCoefficient,
            factor * orientation.ScalarCoefficient);

    /// <inheritdoc cref="operator *(Complex, SianoOrientation)"/>
    public static SianoOrientation operator *(SianoOrientation orientation, Complex factor)
        => factor * orientation;

    /// <summary>
    /// Extends the Siano multiplication table bilinearly to linear combinations.
    /// </summary>
    public static SianoOrientation operator *(SianoOrientation left, SianoOrientation right)
    {
        var (x, y, z, scalar) = left;
        var (otherX, otherY, otherZ, otherScalar) = right;

        return new SianoOrientation(
            x * otherScalar + scalar * otherX + y * otherZ + z * otherY,
            y * otherScalar + scalar * otherY + z * otherX + x * otherZ,
            z * otherScalar + scalar * otherZ + x * otherY + y * otherX,
            scalar * otherScalar + x * otherX + y * otherY + z * otherZ);
    }
}
