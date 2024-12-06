using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("cable", typeof(Length<>))]
[ReferenceUnit(608, UnitType = typeof(Foot))]
public sealed class Cable : Unit;