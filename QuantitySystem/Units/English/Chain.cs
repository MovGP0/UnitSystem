using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("chain", typeof(Length<>))]
[ReferenceUnit(4, UnitType = typeof(Rod))]
public sealed class Chain : Unit;