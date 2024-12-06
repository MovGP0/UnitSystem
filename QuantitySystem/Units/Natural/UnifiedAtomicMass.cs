using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Natural;

/// <summary>
/// Same as Dalton
/// </summary>
[Unit("u", typeof(Mass<>))]
[ReferenceUnit(1, UnitType = typeof(Dalton))]
public sealed class UnifiedAtomicMass : Unit;