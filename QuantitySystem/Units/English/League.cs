using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("league", typeof(Length<>))]
[ReferenceUnit(3, UnitType = typeof(Mile))]
public sealed class League : Unit;