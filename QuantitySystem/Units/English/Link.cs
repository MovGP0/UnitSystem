using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.English;

[Unit("lnk", typeof(Length<>))]
[ReferenceUnit(66, 100, UnitType = typeof(Foot))]
public sealed class Link : Unit;