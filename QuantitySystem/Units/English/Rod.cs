using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("rod", typeof(Length<>))]
[ReferenceUnit(25, UnitType = typeof(Link))]
public sealed class Rod : Unit;