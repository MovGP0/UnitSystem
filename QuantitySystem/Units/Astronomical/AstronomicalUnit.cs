using QuantitySystem.Attributes;
using QuantitySystem.Quantities.BaseQuantities;

namespace QuantitySystem.Units.Astronomical;

[Unit("au", typeof(Length<>))]
[ReferenceUnit(1.495978706916E+11)]
public sealed class AstronomicalUnit : Unit;