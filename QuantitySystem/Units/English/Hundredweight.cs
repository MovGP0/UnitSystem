using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("cwt", typeof(Mass<>))]
[ReferenceUnit(112, UnitType = typeof(Pound))]
public sealed class Hundredweight : Unit;