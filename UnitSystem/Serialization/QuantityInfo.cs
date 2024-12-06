using System.Collections.ObjectModel;

namespace UnitSystem.Serialization;

/// <summary>
/// Represents information about a quantity, including its amount and unit components.
/// </summary>
/// <typeparam name="T">The type of the quantity amount, which must be a struct.</typeparam>
public struct QuantityInfo<T>
    where T : struct
{
    /// <summary>
    /// Initializes a new instance of the <see cref="QuantityInfo{T}"/> struct.
    /// </summary>
    /// <param name="amount">The amount of the quantity.</param>
    /// <param name="unit">A dictionary representing the unit components, where keys are unit symbols and values are their corresponding exponents.</param>
    public QuantityInfo(T amount, IDictionary<string, float> unit)
    {
        Amount = amount;
        Unit = new ReadOnlyDictionary<string, float>(unit);
    }

    /// <summary>
    /// Gets the amount of the quantity.
    /// </summary>
    public T Amount { get; }

    /// <summary>
    /// Gets a dictionary representing the unit components, where keys are unit symbols and values are their corresponding exponents.
    /// </summary>
    public ReadOnlyDictionary<string, float> Unit { get; }
}
