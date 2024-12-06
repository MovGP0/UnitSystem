using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("gr", typeof(Mass<>))]
[ReferenceUnit(1, 7000, UnitType = typeof(Pound))]
public sealed class Grain : Unit;