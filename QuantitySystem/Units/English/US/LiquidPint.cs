using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English.US;

[Unit("lpt", typeof(Volume<>))]
[ReferenceUnit(0.83267418463, UnitType=typeof(Pint))]
public sealed class LiquidPint : Unit;