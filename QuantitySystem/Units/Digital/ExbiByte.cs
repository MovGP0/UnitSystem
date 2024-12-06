using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("EiB", typeof(Digital<>))]
[ReferenceUnit(1024, UnitType = typeof(PebiByte))]
public sealed class ExbiByte : Unit;