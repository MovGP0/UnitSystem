using UnitSystem.Extensions;
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
        var symbols = info.Unit.Keys.ToArray();
        var exponents = info.Unit.Values.ToArray();

        for (var i = 0; i < info.Unit.Count; i++)
        {
            var baseUnit = system[symbols[i]];

            if (baseUnit is not { IsCoherent: true })
            {
                throw new InvalidOperationException(Messages.UnitsNotSameSystem);
            }

            unit *= baseUnit ^ exponents[i];
        }

        return new ScalarQuantity(info.Amount, unit);
    }

    /// <summary>
    /// Serializes an <see cref="Quantity{T}"/> into a <see cref="QuantityInfo{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the quantity amount.</typeparam>
    /// <param name="scalarQuantity">The quantity to serialize.</param>
    /// <returns>A <see cref="QuantityInfo{T}"/> representing the serialized quantity.</returns>
    public static QuantityInfo<T> ToInfo<T>(this Quantity<T> scalarQuantity)
        where T : struct
    {
        var coherent = scalarQuantity.ToCoherent();
        var baseUnits = scalarQuantity.Unit.System.BaseUnits;

        var units = baseUnits.ZipMerge(coherent.Unit.Dimension, (u, exp) => new { u!.Symbol, exp }, true)
            .ToDictionary(u => u.Symbol, u => u.exp);

        return new QuantityInfo<T>(coherent.Amount, units);
    }
}
