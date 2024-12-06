using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English;

[Unit("gal", typeof(Volume<>))]
[ReferenceUnit(8, UnitType = typeof(Pint))]
public sealed class Gallon : Unit;