using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("t", typeof(Mass<>))]
[ReferenceUnit(2240, UnitType = typeof(Pound))]
public sealed class Tonne : Unit;