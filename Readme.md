# Unit System

<img src="logo.webp" width=200 height=200 />

This library implements support for arbitrary unit systems, units, and physical quantities.

## Credits

The library is based on the [Physics library](https://github.com/Tsjunne/Physics) by [Pieter Erzeel](https://github.com/Tsjunne).

The following adoptions have been made:
- added documentation
- added fractional powers and numeric logarithms of dimensionless unit scales
- added partial support for binary prefixes
- added support for CGS and imperial unit system
  - based all unit systems on the SI system to facilitate conversion
- added angles as a separate dimension
- added support for relative units like °C and °F

## Standard and custom units

SI, CGS, and Imperial units share a read-only catalog and a common conversion basis. Use
`UnitSystemFactory.CreateSystem` to register units and symbols in a custom catalog.

`ScalarQuantity` stores amounts as `double`.

Temperature quantities default to points on a scale. Pass `isPoint: false` to the
`ScalarQuantity` constructor for a temperature difference. Subtracting two points
produces a difference. Adding a difference to a point produces a point.

Use `Pow(exponent)` on units, dimensions, and scalar quantities. Dimension exponents
are exact rationals. For example, pass `new Rational(1, 3)` from `ShInUeXx.Numerics`
to `SISystem.m.Pow` to get `m^(1/3)`. Unit strings accept both `m^2` and `m^(1/3)`.

`QuantityInfo` writes exact exponents as strings in `RationalUnit` and can still read
records with numeric exponents in `Unit`.

Unit formatting uses readable symbols and expressions where they round-trip in the
same catalog. If a custom symbol would change the parsed result, formatting emits
`@unit <scale> <offset> <exponents...>` using the catalog's base-unit order.

## Siano orientation

`UnitSystem.Orientations.OrientedQuantity` attaches one of Siano's four labels
to a scalar or an individual signed Cartesian component. Each label is its own
inverse, and the product of two distinct axis labels gives the third axis label.
The orientationless label is the identity and applies to scalars such as mass
and time.

`SianoOrientation` represents these labels as basis vectors with complex
coefficients. For example, `3 * SianoOrientation.X + 4 * SianoOrientation.Y`
has coefficients `(3, 4, 0, 0)` in X, Y, Z, scalar order. Its multiplication
extends the Siano table to sums by distributivity. Geometric dot and cross
products use their own rules.

`OrientedQuantity` accepts a single basis label per component. For a displacement
of `(3, 4, 0) m`, attach X to `3 m` and Y to `4 m`. Express both vectors in a
common frame and add corresponding components when adding vectors. Components
must have matching labels and compatible units for addition and subtraction.

```csharp
using UnitSystem.Orientations;
using UnitSystem.Quantities;
using static UnitSystem.Systems.SISystem;

var displacement = new OrientedQuantity(new ScalarQuantity(2, m), SianoOrientation.X);
var forceX = new OrientedQuantity(new ScalarQuantity(5, N), SianoOrientation.X);
var forceY = new OrientedQuantity(new ScalarQuantity(5, N), SianoOrientation.Y);

var work = displacement * forceX;    // 10 J, orientationless
var momentZ = displacement * forceY; // 10 N·m, Z orientation
```

The prototype supports integer powers. An even power is orientationless, so its
orientation alone cannot recover the original axis. Use the underlying
`ScalarQuantity` for plain dimensional calculations.

[Siano, Orientational Analysis I](https://doi.org/10.1016/0016-0032%2885%2990031-6)

## Open issues

- Explore oriented angles and tensor components from
  [Siano, Orientational Analysis II](https://doi.org/10.1016/0016-0032%2885%2990032-8).
- Improve unit test coverage & bugfixing

## Related work

- [Physics library](https://github.com/Tsjunne/Physics) by [Pieter Erzeel](https://github.com/Tsjunne)
- [QuantitySystem](https://github.com/ibluesun/QuantitySystem) by [Ahmed Sadek Mohamed Tawfik](https://github.com/ibluesun)
- [Units.NET](https://github.com/angularsen/UnitsNet) by [Andreas Gullberg Larsen](https://github.com/angularsen)
- [Quantity.rs](https://github.com/mamrhein/quantities.rs) by [Michael Amrhein](https://github.com/mamrhein)
