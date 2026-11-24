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

## Standard Model gauge checker prototype

`UnitSystem.Gauge` analyzes the gauge representations of products of fields under
color SU(3), weak SU(2), and weak hypercharge U(1). It tracks exact hypercharge
using the convention `Q = T3 + Y` and reports each irreducible channel and its
multiplicity. [Standard Model representations](https://arxiv.org/abs/1705.01853)

```csharp
using UnitSystem.Gauge;

var product = GaugeExpression.Analyze("conj(Q_L) * H * d_R");
// product.HasSinglet is true. The uncontracted product also has nonsinglet channels.
var invariant = GaugeExpression.Analyze("singlet(conj(Q_L) * H * d_R)");
// invariant.IsGaugeInvariant is true.

var higgsQuartic = GaugeExpression.Analyze("singlet(conj(H) * H) * singlet(conj(H) * H)");
// Each Higgs pair is projected to its own singlet.
```

The grammar accepts products of named fields and up to 64 nested pairs of
parentheses. Use `conj(expression)` to conjugate any subexpression and
`singlet(expression)` to select its unique singlet. The default field names are
`Q_L`, `u_R`, `d_R`, `L_L`, `e_R`, and `H`. [FParsec.CSharp](https://github.com/bert2/FParsec.CSharp)

Call `Parse` once and run `Analyze` with a field map to check the same expression
against different representations. `StandardModelGaugeRepresentations` is a
C# 15 union of the six built-in multiplets. Its `Representation` property gives
the gauge representation for the active case. Pass it to `GaugeProduct.Analyze`,
which also accepts custom representations. Color inputs may be singlets,
triplets, antitriplets, or octets.

Singlet selection is symbolic and requires exactly one independent contraction.
The prototype checks internal gauge symmetry. Lorentz indices and field
statistics require their own checks.

## Open issues

- Explore oriented angles and tensor components from
  [Siano, Orientational Analysis II](https://doi.org/10.1016/0016-0032%2885%2990032-8).
- Improve unit test coverage & bugfixing

## Related work

- [Physics library](https://github.com/Tsjunne/Physics) by [Pieter Erzeel](https://github.com/Tsjunne)
- [QuantitySystem](https://github.com/ibluesun/QuantitySystem) by [Ahmed Sadek Mohamed Tawfik](https://github.com/ibluesun)
- [Units.NET](https://github.com/angularsen/UnitsNet) by [Andreas Gullberg Larsen](https://github.com/angularsen)
- [Quantity.rs](https://github.com/mamrhein/quantities.rs) by [Michael Amrhein](https://github.com/mamrhein)
