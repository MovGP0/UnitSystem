using System.Globalization;

namespace QuantitySystem.Units;

public sealed partial class UnitPathItem
{
    public Unit Unit { get; set; }

    public double Times => Numerator / Denominator;

    public double Numerator { get; set; }

    public double Denominator { get; set; }

    public bool IsInverted => Unit.IsInverted;

    /// <summary>
    /// Invert the item with its underlying unit.
    /// </summary>
    public void Invert()
    {
        (Numerator, Denominator) = (Denominator, Numerator);
        Unit = Unit.Invert();
    }

    public override string ToString() => Unit.Symbol + ": " + Times.ToString(CultureInfo.InvariantCulture);
}