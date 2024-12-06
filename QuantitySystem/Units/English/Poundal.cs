using QuantitySystem.Attributes;
using QuantitySystem.Quantities;

namespace QuantitySystem.Units.English;

[Unit("pdl", typeof(Force<>))]
[ReferenceUnit(0.031080950171567253, UnitType=typeof(PoundForce))]
public sealed class Poundal : Unit;