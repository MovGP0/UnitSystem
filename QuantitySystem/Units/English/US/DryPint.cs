using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English.US;

[Unit("dpt", typeof(Volume<>))]
[ReferenceUnit(0.96893897192, UnitType = typeof(Pint))]
public sealed class DryPint : Unit;