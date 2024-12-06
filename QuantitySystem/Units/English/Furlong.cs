using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("fur", typeof(Length<>))]
[ReferenceUnit(220, UnitType = typeof(Yard))]
public sealed class Furlong : Unit;