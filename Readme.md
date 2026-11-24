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

Temperature quantities default to points on a scale. Pass `isPoint: false` to the
`ScalarQuantity` constructor for a temperature difference. Subtracting two points
produces a difference. Adding a difference to a point produces a point.

Use `Pow(exponent)` on units, dimensions, and scalar quantities in C#. Unit strings
continue to use `^` for exponents, such as `m^2`.

Unit formatting uses readable symbols and expressions where they round-trip in the
same catalog. If a custom symbol would change the parsed result, formatting emits
`@unit <scale> <offset> <exponents...>` using the catalog's base-unit order.

## Open issues

- Add support for Siano's extensions to support units with directed dimensions
  - [Units of Measurement Wiki: Siano's extension: orientational analysis](https://units.fandom.com/wiki/Dimensional_analysis#Siano's_extension:_orientational_analysis)
  - [Wikipedia: Siano's extension: orientational analysis](https://en.wikipedia.org/wiki/Dimensional_analysis#Siano's_extension:_orientational_analysis)
- Improve unit test coverage & bugfixing

## Related work

- [Physics library](https://github.com/Tsjunne/Physics) by [Pieter Erzeel](https://github.com/Tsjunne)
- [QuantitySystem](https://github.com/ibluesun/QuantitySystem) by [Ahmed Sadek Mohamed Tawfik](https://github.com/ibluesun)
- [Units.NET](https://github.com/angularsen/UnitsNet) by [Andreas Gullberg Larsen](https://github.com/angularsen)
- [Quantity.rs](https://github.com/mamrhein/quantities.rs) by [Michael Amrhein](https://github.com/mamrhein)
