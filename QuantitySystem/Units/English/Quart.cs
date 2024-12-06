using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English;

[Unit("qt", typeof(Volume<>))]
[ReferenceUnit(2, UnitType = typeof(Pint))]
public sealed class Quart : Unit;