namespace QuantitySystem.Attributes;

/// <summary>
/// The base unit attribute for all units.
/// </summary>
/// <remarks>
/// Definitions:
/// <dl>
/// <dt>Quantity</dt>
/// <dd>The type of the value container.</dd>
/// <dt>Value Container</dt>
/// <dd>The generic which hold the value.</dd>
/// <dt>Units Cloud</dt>
/// <dd>set of units refer to the same Quantity by its Dimension in The Same system.</dd>
/// <dt>System of Units</dt>
/// <dd>a set of different Quantities units grouped into known system {namespace} like imperial and SI or even egyptian</dd>
/// </dl>
/// </remarks>
[AttributeUsage(AttributeTargets.Class)]
public class UnitAttribute : Attribute
{
    /// <summary>
    /// Unit Attribute Constructor.
    /// </summary>
    /// <param name="symbol">Symbol used for this unit.</param>
    /// <param name="quantityType">Quantity Type of this unit.</param>
    public UnitAttribute(string symbol, Type quantityType)
    {
        Symbol = symbol;
        QuantityType = quantityType;
    }

    public string Symbol { get; }

    public Type QuantityType { get; }

    public string QuantityTypeName
    {
        get
        {
            var namespaceLength = QuantityType.Namespace?.Length ?? 0;
            return QuantityType.ToString()[(namespaceLength + 1)..].TrimEnd("`1[T]".ToCharArray());
        }
    }
}