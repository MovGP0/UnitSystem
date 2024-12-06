using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

/// <summary>
/// Minute of arc along any meridian.
/// </summary>
[Unit("nmi", typeof(Length<>))]
[ReferenceUnit(2315000, 381, UnitType = typeof(Foot))]
public sealed class NauticalMile : Unit;