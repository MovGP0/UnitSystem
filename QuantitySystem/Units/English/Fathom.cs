using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("ftm", typeof(Length<>))]
[ReferenceUnit(6, UnitType = typeof(Foot))]
public sealed class Fathom : Unit;