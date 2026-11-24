using UnitSystem.Quantities;

namespace UnitSystem.Serialization;

/// <summary>
/// Provides extension methods for serialization and deserialization of quantities.
/// </summary>
public static class SerializationExtensions
{
    /// <summary>
    /// Deserializes a <see cref="QuantityInfo{T}"/> into a <see cref="ScalarQuantity"/> using the specified <see cref="IUnitSystem"/>.
    /// </summary>
    /// <param name="system">The unit system to use for deserialization.</param>
    /// <param name="info">The serialized quantity information.</param>
    /// <returns>A <see cref="ScalarQuantity"/> representing the deserialized quantity.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the units in <paramref name="info"/> are not part of the same coherent unit system.</exception>
    public static ScalarQuantity FromInfo(this IUnitSystem system, QuantityInfo<double> info)
    {
        var unit = system.NoUnit;
        foreach (var component in info.Unit)
        {
            var baseUnit = system[component.Key];

            if (baseUnit is not { IsCoherent: true })
            {
                throw new InvalidOperationException(Messages.UnitsNotSameSystem);
            }

            unit *= baseUnit.Pow(component.Value);
        }

        return new ScalarQuantity(info.Amount, unit, info.IsPoint ?? unit.SupportsPoints);
    }

    /// <summary>
    /// Serializes a <see cref="ScalarQuantity"/> into a <see cref="QuantityInfo{Double}"/>.
    /// </summary>
    /// <param name="scalarQuantity">The quantity to serialize.</param>
    /// <returns>The canonical amount and unit components.</returns>
    public static QuantityInfo<double> ToInfo(this ScalarQuantity scalarQuantity)
    {
        var coherentUnit = scalarQuantity.CoherentUnit;
        var units = coherentUnit.System.BaseUnits
            .Zip(coherentUnit.Dimension, (unit, exponent) => new { unit.Symbol, Exponent = exponent })
            .Where(component => component.Exponent != 0)
            .ToDictionary(component => component.Symbol, component => component.Exponent);

        return new QuantityInfo<double>(scalarQuantity.CoherentAmount, units,
            scalarQuantity.IsPoint || scalarQuantity.Unit.SupportsPoints || coherentUnit.SupportsPoints
                ? scalarQuantity.IsPoint
                : null);
    }
}
