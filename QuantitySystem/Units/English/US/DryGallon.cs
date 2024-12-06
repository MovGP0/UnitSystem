using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English.US;

[Unit("dgal", typeof(Volume<>))]
[ReferenceUnit(8, UnitType = typeof(DryPint))]
public sealed class DryGallon : Unit;