using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Digital;

[Unit("B", typeof(Digital<>))]
[ReferenceUnit(8, UnitType = typeof(Bit))]
public sealed class Byte : Unit;

#region decimal

#endregion

#region Binary

#endregion