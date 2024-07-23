using System.Globalization;
using System.Text;

namespace QuantitySystem;

public sealed partial class QuantityDimension : IFormattable
{
    public override string ToString() => ToString(null, CultureInfo.InvariantCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var dim = new StringBuilder();
        dim.Append("M" + Mass.Exponent.ToString(format, formatProvider));

        if (Length.VectorExponent != 0)
        {
            dim.Append(string.Format("L{0}(S{1}V{2}M{3})",
                    Length.Exponent.ToString(format, formatProvider),
                    Length.ScalarExponent.ToString(format, formatProvider),
                    Length.VectorExponent.ToString(format, formatProvider),
                    Length.MatrixExponent.ToString(format, formatProvider)
                ));
        }
        else
        {
            dim.Append($"L{Length.Exponent.ToString(format, formatProvider)}");
        }

        dim.Append("T" + Time.Exponent.ToString(format, formatProvider));
        dim.Append("I" + ElectricCurrent.Exponent.ToString(format, formatProvider));
        dim.Append("O" + Temperature.Exponent.ToString(format, formatProvider));
        dim.Append("N" + AmountOfSubstance.Exponent.ToString(format, formatProvider));
        dim.Append("J" + LuminousIntensity.Exponent.ToString(format, formatProvider));
        dim.Append("$" + Currency.Exponent.ToString(format, formatProvider));
        dim.Append("D" + Digital.Exponent.ToString(format, formatProvider));
        return dim.ToString();
    }
}