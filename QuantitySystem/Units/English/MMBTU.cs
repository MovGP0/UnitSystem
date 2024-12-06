using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English;

[Unit("MMBTU", typeof(Energy<>))]
[ReferenceUnit(1000000, UnitType = typeof(BTU))]
public sealed class MMBTU : Unit;