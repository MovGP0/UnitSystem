using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English.US;

[Unit("dqt", typeof(Volume<>))]
[ReferenceUnit(2, UnitType = typeof(DryPint))]
public sealed class DryQuart : Unit;