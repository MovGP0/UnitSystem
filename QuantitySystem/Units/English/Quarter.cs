using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("quarter", typeof(Mass<>))]
[ReferenceUnit(28, UnitType = typeof(Pound))]
public sealed class Quarter : Unit;