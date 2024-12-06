using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("in", typeof(Length<>))]
[ReferenceUnit(1.0, 12.0, UnitType = typeof(Foot))]
public sealed class Inch : Unit;