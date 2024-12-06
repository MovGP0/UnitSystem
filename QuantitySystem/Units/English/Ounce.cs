using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("oz", typeof(Mass<>))]
[ReferenceUnit(1, 16, UnitType = typeof(Pound))]
public sealed class Ounce : Unit;