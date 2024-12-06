using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English;

[Unit("gill", typeof(Volume<>))]
[ReferenceUnit(1, 4, UnitType = typeof(Pint))]
public sealed class Gill : Unit;