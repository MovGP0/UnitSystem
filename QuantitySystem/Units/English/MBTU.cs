using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English;

[Unit("MBTU", typeof(Energy<>))]
[ReferenceUnit(1000, UnitType = typeof(BTU))]
public sealed class MBTU : Unit;