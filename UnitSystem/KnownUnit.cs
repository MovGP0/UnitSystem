using UnitSystem.Dimensions;
using UnitSystem.Extensions;
using UnitSystem.Prefixes;

namespace UnitSystem;

/// <summary>
/// Represents a known unit within a specific unit system, including its symbol, name, and inherent factor.
/// </summary>
public sealed class KnownUnit : Unit
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KnownUnit"/> class.
    /// </summary>
    /// <param name="system">The unit system to which the unit belongs.</param>
    /// <param name="factor">The factor by which the unit is multiplied.</param>
    /// <param name="offset">The offset of the unit from zero.</param>
    /// <param name="dimension">The dimension of the unit.</param>
    /// <param name="symbol">The symbol representing the unit.</param>
    /// <param name="name">The name of the unit.</param>
    public KnownUnit(
        IUnitSystem system,
        double factor,
        double offset,
        Dimension dimension,
        string symbol,
        string name)
        : this(system, factor, offset, dimension, symbol, name, string.Empty)
    {
        Symbol = symbol;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="KnownUnit"/> class with the option to specify if the unit has an inherent prefix.
    /// </summary>
    /// <param name="system">The unit system to which the unit belongs.</param>
    /// <param name="factor">The factor by which the unit is multiplied.</param>
    /// <param name="offset">The offset of the unit from zero.</param>
    /// <param name="dimension">The dimension of the unit.</param>
    /// <param name="symbol">The symbol representing the unit.</param>
    /// <param name="name">The name of the unit.</param>
    /// <param name="inherentPrefix">Indicates whether the unit has an inherent prefix.</param>
    internal KnownUnit(
        IUnitSystem system,
        double factor,
        double offset,
        Dimension dimension,
        string symbol,
        string name,
        string inherentPrefix)
        : base(system, factor, offset, dimension)
    {
        Check.Argument(symbol, nameof(symbol)).IsNotNull();
        Check.Argument(name, nameof(name)).IsNotNull();

        Symbol = symbol;
        Name = name;

        if (!string.IsNullOrEmpty(inherentPrefix))
        {
            BaseSymbol = "";
            DeriveInherentFactorAndBaseSymbol(symbol, inherentPrefix);
        }
        else
        {
            InherentFactor = 1;
            BaseSymbol = symbol;
        }
    }

    /// <summary>
    /// Gets the symbol representing the unit.
    /// </summary>
    public string Symbol { get; }

    /// <summary>
    /// Gets the name of the unit.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Gets the inherent factor of the unit, which is determined by the prefix.
    /// </summary>
    public double InherentFactor { get; private set; }

    /// <summary>
    /// Gets the base symbol of the unit, excluding any inherent prefix.
    /// </summary>
    public string BaseSymbol { get; private set; }

    /// <summary>
    /// Returns a string representation of the unit, which is its symbol.
    /// </summary>
    /// <returns>The symbol of the unit.</returns>
    public override string ToString() => Symbol;

    /// <summary>
    /// Derives the inherent factor and base symbol of the unit from its symbol if it has an inherent prefix.
    /// </summary>
    /// <param name="symbol">The symbol representing the unit.</param>
    /// <param name="prefix">The inherent prefix of the unit.</param>
    private void DeriveInherentFactorAndBaseSymbol(string symbol, string prefix)
    {
        var baseSymbol = symbol.Substring(prefix.Length);

        if (string.IsNullOrEmpty(baseSymbol))
        {
            ThrowArgumentException();
        }

        if (!MetricPrefix.TryGetValue(prefix, out var inherentPrefix))
        {
            ThrowArgumentException();
        }

        InherentFactor = inherentPrefix.Factor;
        BaseSymbol = baseSymbol;
        return;

        void ThrowArgumentException() => throw new ArgumentException(Messages.InherentPrefixInvalid.FormatWith(symbol));
    }
}
